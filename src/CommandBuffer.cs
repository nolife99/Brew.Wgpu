using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>A recorded command buffer; crosses to the submit thread, then is disposed by the submitter. The handle is a value type ({slot, generation}); copies are free, disposal releases exactly once and invalidates all copies.</summary>
public readonly struct CommandBuffer : IDisposable, IEquatable<CommandBuffer>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe CommandBuffer(WGPUCommandBufferImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUCommandBufferImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUCommandBufferImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUCommandBufferImpl* Handle
    {
        get
        {
            WGPUCommandBufferImpl* p = HandleTable<WGPUCommandBufferImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(CommandBuffer));
            return p;
        }
    }

    public unsafe void Dispose()
    {
        WGPUCommandBufferImpl* p = HandleTable<WGPUCommandBufferImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuCommandBufferRelease(p);
    }

    public bool Equals(CommandBuffer other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is CommandBuffer o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(CommandBuffer left, CommandBuffer right) => left.Equals(right);
    public static bool operator !=(CommandBuffer left, CommandBuffer right) => !left.Equals(right);
}
