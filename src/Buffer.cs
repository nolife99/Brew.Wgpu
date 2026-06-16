using Brew.Wgpu.Handles;
using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Brew.Wgpu;

public readonly struct Buffer : IDisposable, IEquatable<Buffer>
{
    private readonly int _slot;
    private readonly uint _gen;

    private Buffer(int slot, uint gen)
    {
        _slot = slot;
        _gen = gen;
    }

    /// <summary>Wraps a freshly-created native handle, registering it in the handle table.</summary>
    internal unsafe Buffer(WGPUBufferImpl* handle)
    {
        if (handle is null)
        {
            _slot = 0;
            _gen = 0;
        }
        else
        {
            (_slot, _gen) = HandleTable<WGPUBufferImpl>.Register(handle);
        }
    }

    /// <summary>Adopt a freshly-created native handle into the table. Internal: callers obtain a <see cref="Buffer"/> from <see cref="Device"/>.</summary>
    internal static unsafe Buffer Adopt(WGPUBufferImpl* handle)
    {
        if (handle is null)
            return default;
        (int slot, uint gen) = HandleTable<WGPUBufferImpl>.Register(handle);
        return new Buffer(slot, gen);
    }

    /// <summary>True for a default, never-assigned, or already-disposed handle.</summary>
    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUBufferImpl>.Resolve(_slot, _gen) is null;

    /// <summary>The live native pointer. Throws <see cref="ObjectDisposedException"/> if the buffer has been disposed.</summary>
    public unsafe WGPUBufferImpl* Handle
    {
        get
        {
            WGPUBufferImpl* p = HandleTable<WGPUBufferImpl>.Resolve(_slot, _gen);
            if (p is null)
                throw new ObjectDisposedException(nameof(Buffer));
            return p;
        }
    }

    /// <summary>Maps the buffer and blocks, pumping <paramref name="instance"/>, until the map resolves or times out.</summary>
    public unsafe WGPUMapAsyncStatus MapBlocking(Instance instance, MapMode mode, ulong offset, nuint size)
    {
        var status = (WGPUMapAsyncStatus)int.MaxValue;
        var callbackInfo = new WGPUBufferMapCallbackInfo
        {
            mode = WGPUCallbackMode.AllowProcessEvents,
            callback = &OnMap,
            userdata1 = Unsafe.AsPointer(ref status),
        };
        WGPU.wgpuBufferMapAsync(Handle, (ulong)mode, (nuint)offset, size, callbackInfo);
        Async.PollUntilChanged(instance.Handle, ref status, (WGPUMapAsyncStatus)int.MaxValue);
        return status;
    }

    /// <summary>Starts an async map and returns a request whose status slot is filled in when the map resolves.</summary>
    public unsafe BufferMapRequest BeginMap(MapMode mode, ulong offset, nuint size)
    {
        var slot = (WGPUMapAsyncStatus*)NativeMemory.Alloc((nuint)sizeof(WGPUMapAsyncStatus));
        *(int*)slot = int.MaxValue;
        var callbackInfo = new WGPUBufferMapCallbackInfo
        {
            mode = WGPUCallbackMode.AllowProcessEvents,
            callback = &OnMap,
            userdata1 = slot,
        };
        WGPU.wgpuBufferMapAsync(Handle, (ulong)mode, (nuint)offset, size, callbackInfo);
        return new BufferMapRequest(slot);
    }

    /// <summary>Returns the mapped range as a read-only span of <typeparamref name="T"/>. The buffer must be mapped.</summary>
    public unsafe ReadOnlySpan<T> GetConstMappedRange<T>(ulong offset, nuint size) where T : unmanaged
    {
        void* p = WGPU.wgpuBufferGetConstMappedRange(Handle, (nuint)offset, size);
        if (p is null)
            throw new InvalidOperationException("wgpuBufferGetConstMappedRange returned null — buffer may not be mapped.");
        return new ReadOnlySpan<T>(p, (int)(size / (nuint)sizeof(T)));
    }

    /// <summary>Returns the mapped range as a writable span of <typeparamref name="T"/>. The buffer must be mapped for writes.</summary>
    public unsafe Span<T> GetMappedRange<T>(ulong offset, nuint size) where T : unmanaged
    {
        void* p = WGPU.wgpuBufferGetMappedRange(Handle, (nuint)offset, size);
        if (p is null)
            throw new InvalidOperationException("wgpuBufferGetMappedRange returned null — buffer may not be mapped for writes.");
        return new Span<T>(p, (int)(size / (nuint)sizeof(T)));
    }

    /// <summary>Unmaps a previously-mapped buffer.</summary>
    public unsafe void Unmap() => WGPU.wgpuBufferUnmap(Handle);

    /// <summary>Releases the buffer. Safe to call repeatedly and on copies — only the first call performs the native release.</summary>
    public unsafe void Dispose()
    {
        WGPUBufferImpl* p = HandleTable<WGPUBufferImpl>.Retire(_slot, _gen);
        if (p is not null)
            WGPU.wgpuBufferRelease(p);
    }

    public bool Equals(Buffer other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is Buffer other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(Buffer left, Buffer right) => left.Equals(right);
    public static bool operator !=(Buffer left, Buffer right) => !left.Equals(right);

    // Map-status callback: write the resolved status into the caller-provided slot.
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void OnMap(WGPUMapAsyncStatus status, WGPUStringView message, void* userdata1, void* userdata2)
        => *(int*)userdata1 = (int)status;
}
