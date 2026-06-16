using Brew.Wgpu.Handles;
using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Brew.Wgpu;

public readonly struct RenderPassEncoder : IDisposable, IEquatable<RenderPassEncoder>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe RenderPassEncoder(WGPURenderPassEncoderImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPURenderPassEncoderImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPURenderPassEncoderImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after <see cref="End"/>/<see cref="Dispose"/>.</summary>
    public unsafe WGPURenderPassEncoderImpl* Handle
    {
        get
        {
            WGPURenderPassEncoderImpl* p = HandleTable<WGPURenderPassEncoderImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(RenderPassEncoder));
            return p;
        }
    }

    public readonly unsafe void SetPipeline(RenderPipeline pipeline)
    {
        WGPU.wgpuRenderPassEncoderSetPipeline(this.Handle, pipeline.Handle);
    }

    public readonly unsafe void SetViewport(
      float x,
      float y,
      float width,
      float height,
      float minDepth = 0.0f,
      float maxDepth = 1f)
    {
        WGPU.wgpuRenderPassEncoderSetViewport(this.Handle, x, y, width, height, minDepth, maxDepth);
    }

    public readonly unsafe void SetScissorRect(uint x, uint y, uint width, uint height)
    {
        WGPU.wgpuRenderPassEncoderSetScissorRect(this.Handle, x, y, width, height);
    }

    public readonly unsafe void SetBindGroup(
      uint groupIndex,
      BindGroup group,
      scoped ReadOnlySpan<uint> dynamicOffsets = default(ReadOnlySpan<uint>))
    {
        fixed (uint* numPtr = &dynamicOffsets.GetPinnableReference())
            WGPU.wgpuRenderPassEncoderSetBindGroup(this.Handle, groupIndex, group.Handle, (UIntPtr)dynamicOffsets.Length, numPtr);
    }

    public readonly unsafe void Draw(
      uint vertexCount,
      uint instanceCount = 1,
      uint firstVertex = 0,
      uint firstInstance = 0)
    {
        WGPU.wgpuRenderPassEncoderDraw(this.Handle, vertexCount, instanceCount, firstVertex, firstInstance);
    }

    public readonly unsafe void SetVertexBuffer(uint slot, Buffer buffer, ulong offset = 0, ulong size = 18446744073709551615 /*0xFFFFFFFFFFFFFFFF*/)
    {
        WGPU.wgpuRenderPassEncoderSetVertexBuffer(this.Handle, slot, buffer.Handle, offset, size);
    }

    public readonly unsafe void SetIndexBuffer(
      Buffer buffer,
      WGPUIndexFormat format,
      ulong offset = 0,
      ulong size = 18446744073709551615 /*0xFFFFFFFFFFFFFFFF*/)
    {
        WGPU.wgpuRenderPassEncoderSetIndexBuffer(this.Handle, buffer.Handle, format, offset, size);
    }

    public readonly unsafe void DrawIndexed(
      uint indexCount,
      uint instanceCount = 1,
      uint firstIndex = 0,
      int baseVertex = 0,
      uint firstInstance = 0)
    {
        WGPU.wgpuRenderPassEncoderDrawIndexed(this.Handle, indexCount, instanceCount, firstIndex, baseVertex, firstInstance);
    }

    public readonly unsafe void DrawIndirect(Buffer indirectBuffer, ulong indirectOffset = 0)
    {
        WGPU.wgpuRenderPassEncoderDrawIndirect(this.Handle, indirectBuffer.Handle, indirectOffset);
    }

    public readonly unsafe void DrawIndexedIndirect(Buffer indirectBuffer, ulong indirectOffset = 0)
    {
        WGPU.wgpuRenderPassEncoderDrawIndexedIndirect(this.Handle, indirectBuffer.Handle, indirectOffset);
    }

    public readonly unsafe void MultiDrawIndexedIndirect(
      Buffer indirectBuffer,
      ulong offset,
      uint count)
    {
        WGPU.wgpuRenderPassEncoderMultiDrawIndexedIndirect(this.Handle, indirectBuffer.Handle, offset, count);
    }

    public readonly unsafe void SetPushConstants<T>(uint offset, scoped ReadOnlySpan<T> data) where T : unmanaged
    {
        ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.AsBytes<T>(data);
        fixed (byte* numPtr = &readOnlySpan.GetPinnableReference())
            WGPU.wgpuRenderPassEncoderSetImmediates(this.Handle, offset, (uint)readOnlySpan.Length, (void*)numPtr);
    }

    public readonly unsafe void MultiDrawIndexedIndirectCount(
      Buffer indirectBuffer,
      ulong indirectOffset,
      Buffer countBuffer,
      ulong countOffset,
      uint maxCount)
    {
        WGPU.wgpuRenderPassEncoderMultiDrawIndexedIndirectCount(this.Handle, indirectBuffer.Handle, indirectOffset, countBuffer.Handle, countOffset, maxCount);
    }

    public readonly unsafe void PushDebugGroup(scoped ReadOnlySpan<byte> label)
    {
        fixed (byte* data = &label.GetPinnableReference())
            WGPU.wgpuRenderPassEncoderPushDebugGroup(this.Handle, WgpuDefaults.OptionalStringView(data, label.Length));
    }

    public readonly unsafe void PopDebugGroup()
    {
        WGPU.wgpuRenderPassEncoderPopDebugGroup(this.Handle);
    }

    public readonly unsafe void InsertDebugMarker(scoped ReadOnlySpan<byte> label)
    {
        fixed (byte* data = &label.GetPinnableReference())
            WGPU.wgpuRenderPassEncoderInsertDebugMarker(this.Handle, WgpuDefaults.OptionalStringView(data, label.Length));
    }

    /// <summary>
    /// Ends the render pass and releases its native encoder. Idempotent: the
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
        WGPURenderPassEncoderImpl* p = HandleTable<WGPURenderPassEncoderImpl>.Retire(_slot, _gen);
        if (p is null)
            return;
        WGPU.wgpuRenderPassEncoderEnd(p);
        WGPU.wgpuRenderPassEncoderRelease(p);
    }

    /// <summary>Equivalent to <see cref="End"/>; present so the pass works with <c>using</c>.</summary>
    public unsafe void Dispose() => End();

    public bool Equals(RenderPassEncoder other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object obj) => obj is RenderPassEncoder o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(RenderPassEncoder left, RenderPassEncoder right) => left.Equals(right);
    public static bool operator !=(RenderPassEncoder left, RenderPassEncoder right) => !left.Equals(right);
}
