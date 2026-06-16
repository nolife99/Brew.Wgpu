using Brew.Wgpu.Handles;
using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Brew.Wgpu;

public readonly struct ComputePassEncoder : IDisposable, IEquatable<ComputePassEncoder>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe ComputePassEncoder(WGPUComputePassEncoderImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUComputePassEncoderImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUComputePassEncoderImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after <see cref="End"/>/<see cref="Dispose"/>.</summary>
    public unsafe WGPUComputePassEncoderImpl* Handle
    {
        get
        {
            WGPUComputePassEncoderImpl* p = HandleTable<WGPUComputePassEncoderImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(ComputePassEncoder));
            return p;
        }
    }

    public readonly unsafe void SetPipeline(ComputePipeline pipeline)
    {
        WGPU.wgpuComputePassEncoderSetPipeline(this.Handle, pipeline.Handle);
    }

    public readonly unsafe void SetBindGroup(
      uint groupIndex,
      BindGroup group,
      scoped ReadOnlySpan<uint> dynamicOffsets = default(ReadOnlySpan<uint>))
    {
        fixed (uint* numPtr = &dynamicOffsets.GetPinnableReference())
            WGPU.wgpuComputePassEncoderSetBindGroup(this.Handle, groupIndex, group.Handle, (UIntPtr)dynamicOffsets.Length, numPtr);
    }

    public readonly unsafe void DispatchWorkgroups(uint x, uint y = 1, uint z = 1)
    {
        WGPU.wgpuComputePassEncoderDispatchWorkgroups(this.Handle, x, y, z);
    }

    public readonly unsafe void DispatchWorkgroupsIndirect(
      Buffer indirectBuffer,
      ulong indirectOffset = 0)
    {
        WGPU.wgpuComputePassEncoderDispatchWorkgroupsIndirect(this.Handle, indirectBuffer.Handle, indirectOffset);
    }

    public readonly unsafe void SetPushConstants<T>(uint offset, scoped ReadOnlySpan<T> data) where T : unmanaged
    {
        ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.AsBytes<T>(data);
        fixed (byte* numPtr = &readOnlySpan.GetPinnableReference())
            WGPU.wgpuComputePassEncoderSetImmediates(this.Handle, offset, (uint)readOnlySpan.Length, (void*)numPtr);
    }

    public readonly unsafe void PushDebugGroup(scoped ReadOnlySpan<byte> label)
    {
        fixed (byte* data = &label.GetPinnableReference())
            WGPU.wgpuComputePassEncoderPushDebugGroup(this.Handle, WgpuDefaults.OptionalStringView(data, label.Length));
    }

    public readonly unsafe void PopDebugGroup()
    {
        WGPU.wgpuComputePassEncoderPopDebugGroup(this.Handle);
    }

    public readonly unsafe void InsertDebugMarker(scoped ReadOnlySpan<byte> label)
    {
        fixed (byte* data = &label.GetPinnableReference())
            WGPU.wgpuComputePassEncoderInsertDebugMarker(this.Handle, WgpuDefaults.OptionalStringView(data, label.Length));
    }

    /// <summary>
    /// Ends the compute pass and releases its native encoder. Idempotent: the
    /// generation bump in <see cref="HandleTable{T}.Retire"/> means a second
    /// <c>End</c> (or a <c>Dispose</c> after <c>End</c>) is a safe no-op, which is
    /// what the old mutable <c>_ended</c> flag used to provide. Recording on the
    /// pass after this throws <see cref="ObjectDisposedException"/> instead of
    /// reaching freed native memory.
    /// </summary>
    public unsafe void End()
    {
        // Retire claims the pointer exactly once; the native object stays alive until
        // Release, so ending then releasing the claimed pointer is valid.
        WGPUComputePassEncoderImpl* p = HandleTable<WGPUComputePassEncoderImpl>.Retire(_slot, _gen);
        if (p is null)
            return;
        WGPU.wgpuComputePassEncoderEnd(p);
        WGPU.wgpuComputePassEncoderRelease(p);
    }

    /// <summary>Equivalent to <see cref="End"/>; present so the pass works with <c>using</c>.</summary>
    public unsafe void Dispose() => End();

    public bool Equals(ComputePassEncoder other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object obj) => obj is ComputePassEncoder o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(ComputePassEncoder left, ComputePassEncoder right) => left.Equals(right);
    public static bool operator !=(ComputePassEncoder left, ComputePassEncoder right) => !left.Equals(right);
}
