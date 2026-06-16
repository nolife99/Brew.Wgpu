using Brew.Wgpu.Handles;
using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Brew.Wgpu;

public readonly struct CommandEncoder : IDisposable, IEquatable<CommandEncoder>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe CommandEncoder(WGPUCommandEncoderImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUCommandEncoderImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUCommandEncoderImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after <see cref="Finish"/>/<see cref="Dispose"/>.</summary>
    public unsafe WGPUCommandEncoderImpl* Handle
    {
        get
        {
            WGPUCommandEncoderImpl* p = HandleTable<WGPUCommandEncoderImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(CommandEncoder));
            return p;
        }
    }

    public RenderPassEncoder BeginRenderPass(
      scoped ReadOnlySpan<ColorAttachment> colorAttachments,
      scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        return this.BeginRenderPass(colorAttachments, new DepthStencilAttachment(), label);
    }

    public unsafe RenderPassEncoder BeginRenderPass(
      scoped ReadOnlySpan<ColorAttachment> colorAttachments,
      scoped in DepthStencilAttachment depthStencil,
      scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        int length = colorAttachments.Length;
        Span<WGPURenderPassColorAttachment> span = stackalloc WGPURenderPassColorAttachment[length];
        for (int index = 0; index < length; ++index)
        {
            ref readonly ColorAttachment local = ref colorAttachments[index];
            span[index] = new WGPURenderPassColorAttachment()
            {
                view = local.View.Handle,
                resolveTarget = local.ResolveTarget.Handle,
                loadOp = local.LoadOp,
                storeOp = local.StoreOp,
                clearValue = local.ClearValue,
                depthSlice = uint.MaxValue
            };
        }
        bool flag = !depthStencil.View.IsNull;
        WGPURenderPassDepthStencilAttachment stencilAttachment = new WGPURenderPassDepthStencilAttachment();
        if (flag)
            stencilAttachment = new WGPURenderPassDepthStencilAttachment()
            {
                view = depthStencil.View.Handle,
                depthLoadOp = depthStencil.DepthLoadOp,
                depthStoreOp = depthStencil.DepthStoreOp,
                depthClearValue = depthStencil.DepthClearValue,
                depthReadOnly = depthStencil.DepthReadOnly ? 1u : 0u,
                stencilLoadOp = depthStencil.StencilLoadOp,
                stencilStoreOp = depthStencil.StencilStoreOp,
                stencilClearValue = depthStencil.StencilClearValue,
                stencilReadOnly = depthStencil.StencilReadOnly ? 1u : 0u
            };
        label = new ReadOnlySpan<byte>();
        fixed (WGPURenderPassColorAttachment* passColorAttachmentPtr = &span.GetPinnableReference())
        fixed (byte* data = &label.GetPinnableReference())
        {
            var __h0 = new WGPURenderPassDescriptor()
            {
                label = WgpuDefaults.OptionalStringView(data, label.Length),
                colorAttachmentCount = (UIntPtr)length,
                colorAttachments = passColorAttachmentPtr,
                depthStencilAttachment = (flag ? &stencilAttachment : (WGPURenderPassDepthStencilAttachment*)null)
            };
            WGPURenderPassEncoderImpl* handle = WGPU.wgpuCommandEncoderBeginRenderPass(this.Handle, &__h0);
            return (IntPtr)handle != IntPtr.Zero ? new RenderPassEncoder(handle) : throw new ResourceCreationException("RenderPassEncoder", "wgpuCommandEncoderBeginRenderPass");
        }
    }

    public unsafe ComputePassEncoder BeginComputePass(scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        label = new ReadOnlySpan<byte>();
        fixed (byte* data = &label.GetPinnableReference())
        {
            var __h1 = new WGPUComputePassDescriptor()
            {
                label = WgpuDefaults.OptionalStringView(data, label.Length)
            };
            WGPUComputePassEncoderImpl* pass = WGPU.wgpuCommandEncoderBeginComputePass(this.Handle, &__h1);
            return (IntPtr)pass != IntPtr.Zero ? new ComputePassEncoder(pass) : throw new ResourceCreationException("ComputePassEncoder", "wgpuCommandEncoderBeginComputePass");
        }
    }

    public unsafe void CopyTextureToBuffer(
      Texture source,
      Buffer destination,
      uint bytesPerRow,
      uint rowsPerImage,
      scoped in WGPUExtent3D size,
      uint mipLevel = 0,
      ulong bufferOffset = 0)
    {
        WGPUTexelCopyTextureInfo texelCopyTextureInfo = new WGPUTexelCopyTextureInfo()
        {
            texture = source.Handle,
            mipLevel = mipLevel
        };
        WGPUTexelCopyBufferInfo texelCopyBufferInfo = new WGPUTexelCopyBufferInfo()
        {
            buffer = destination.Handle,
            layout = new WGPUTexelCopyBufferLayout()
            {
                offset = bufferOffset,
                bytesPerRow = bytesPerRow,
                rowsPerImage = rowsPerImage
            }
        };
        fixed (WGPUExtent3D* wgpuExtent3DPtr = &Unsafe.AsRef<WGPUExtent3D>(in size))
            WGPU.wgpuCommandEncoderCopyTextureToBuffer(this.Handle, &texelCopyTextureInfo, &texelCopyBufferInfo, wgpuExtent3DPtr);
    }

    public unsafe void CopyBufferToBuffer(
      Buffer source,
      ulong sourceOffset,
      Buffer destination,
      ulong destinationOffset,
      ulong size)
    {
        WGPU.wgpuCommandEncoderCopyBufferToBuffer(this.Handle, source.Handle, sourceOffset, destination.Handle, destinationOffset, size);
    }

    public unsafe void CopyBufferToTexture(
      Buffer source,
      uint bytesPerRow,
      uint rowsPerImage,
      Texture destination,
      scoped in WGPUExtent3D size,
      uint destMipLevel = 0,
      ulong bufferOffset = 0)
    {
        WGPUTexelCopyBufferInfo texelCopyBufferInfo = new WGPUTexelCopyBufferInfo()
        {
            buffer = source.Handle,
            layout = new WGPUTexelCopyBufferLayout()
            {
                offset = bufferOffset,
                bytesPerRow = bytesPerRow,
                rowsPerImage = rowsPerImage
            }
        };
        WGPUTexelCopyTextureInfo texelCopyTextureInfo = new WGPUTexelCopyTextureInfo()
        {
            texture = destination.Handle,
            mipLevel = destMipLevel
        };
        fixed (WGPUExtent3D* wgpuExtent3DPtr = &Unsafe.AsRef<WGPUExtent3D>(in size))
            WGPU.wgpuCommandEncoderCopyBufferToTexture(this.Handle, &texelCopyBufferInfo, &texelCopyTextureInfo, wgpuExtent3DPtr);
    }

    public unsafe void CopyTextureToTexture(
      Texture source,
      Texture destination,
      scoped in WGPUExtent3D size,
      uint srcMipLevel = 0,
      uint dstMipLevel = 0)
    {
        WGPUTexelCopyTextureInfo texelCopyTextureInfo1 = new WGPUTexelCopyTextureInfo()
        {
            texture = source.Handle,
            mipLevel = srcMipLevel
        };
        WGPUTexelCopyTextureInfo texelCopyTextureInfo2 = new WGPUTexelCopyTextureInfo()
        {
            texture = destination.Handle,
            mipLevel = dstMipLevel
        };
        fixed (WGPUExtent3D* wgpuExtent3DPtr = &Unsafe.AsRef<WGPUExtent3D>(in size))
            WGPU.wgpuCommandEncoderCopyTextureToTexture(this.Handle, &texelCopyTextureInfo1, &texelCopyTextureInfo2, wgpuExtent3DPtr);
    }

    public unsafe void ClearTexture(
      TextureView view,
      in WGPUColor clearColor,
      scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        WGPURenderPassColorAttachment passColorAttachment = new WGPURenderPassColorAttachment()
        {
            view = view.Handle,
            resolveTarget = (WGPUTextureViewImpl*)null,
            loadOp = (WGPULoadOp)2,
            storeOp = (WGPUStoreOp)1,
            clearValue = clearColor,
            depthSlice = uint.MaxValue
        };
        label = new ReadOnlySpan<byte>();
        fixed (byte* data = &label.GetPinnableReference())
        {
            var __h2 = new WGPURenderPassDescriptor()
            {
                label = WgpuDefaults.OptionalStringView(data, label.Length),
                colorAttachmentCount = new UIntPtr(1),
                colorAttachments = &passColorAttachment,
                depthStencilAttachment = (WGPURenderPassDepthStencilAttachment*)null
            };
            WGPURenderPassEncoderImpl* renderPassEncoderImplPtr = WGPU.wgpuCommandEncoderBeginRenderPass(this.Handle, &__h2);
            if ((IntPtr)renderPassEncoderImplPtr == IntPtr.Zero)
                throw new ResourceCreationException("RenderPassEncoder", "wgpuCommandEncoderBeginRenderPass (ClearTexture)");
            WGPU.wgpuRenderPassEncoderEnd(renderPassEncoderImplPtr);
            WGPU.wgpuRenderPassEncoderRelease(renderPassEncoderImplPtr);
        }
    }

    public void ClearTextureUint(
      TextureView view,
      uint r,
      uint g,
      uint b,
      uint a,
      scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        WGPUColor clearColor = new WGPUColor()
        {
            r = (double)r,
            g = (double)g,
            b = (double)b,
            a = (double)a
        };
        this.ClearTexture(view, in clearColor, label);
    }

    public void ClearTextureSint(
      TextureView view,
      int r,
      int g,
      int b,
      int a,
      scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        WGPUColor clearColor = new WGPUColor()
        {
            r = (double)r,
            g = (double)g,
            b = (double)b,
            a = (double)a
        };
        this.ClearTexture(view, in clearColor, label);
    }

    public void ClearDepth(TextureView view, float depth, scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        this.ClearDepthStencilCore(view, true, depth, false, 0U, label);
    }

    public void ClearStencil(TextureView view, uint stencil, scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        this.ClearDepthStencilCore(view, false, 0.0f, true, stencil, label);
    }

    public void ClearDepthStencil(
      TextureView view,
      float depth,
      uint stencil,
      scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        this.ClearDepthStencilCore(view, true, depth, true, stencil, label);
    }

    private unsafe void ClearDepthStencilCore(
      TextureView view,
      bool hasDepth,
      float depth,
      bool hasStencil,
      uint stencil,
      scoped ReadOnlySpan<byte> label)
    {
        WGPURenderPassDepthStencilAttachment stencilAttachment = new WGPURenderPassDepthStencilAttachment()
        {
            view = view.Handle,
            depthLoadOp = hasDepth ? (WGPULoadOp)2 : (WGPULoadOp)0,
            depthStoreOp = hasDepth ? (WGPUStoreOp)1 : (WGPUStoreOp)0,
            depthClearValue = depth,
            depthReadOnly = 0,
            stencilLoadOp = hasStencil ? (WGPULoadOp)2 : (WGPULoadOp)0,
            stencilStoreOp = hasStencil ? (WGPUStoreOp)1 : (WGPUStoreOp)0,
            stencilClearValue = stencil,
            stencilReadOnly = 0
        };
        label = new ReadOnlySpan<byte>();
        fixed (byte* data = &label.GetPinnableReference())
        {
            var __h3 = new WGPURenderPassDescriptor()
            {
                label = WgpuDefaults.OptionalStringView(data, label.Length),
                colorAttachmentCount = UIntPtr.Zero,
                colorAttachments = (WGPURenderPassColorAttachment*)null,
                depthStencilAttachment = &stencilAttachment
            };
            WGPURenderPassEncoderImpl* renderPassEncoderImplPtr = WGPU.wgpuCommandEncoderBeginRenderPass(this.Handle, &__h3);
            if ((IntPtr)renderPassEncoderImplPtr == IntPtr.Zero)
                throw new ResourceCreationException("RenderPassEncoder", "wgpuCommandEncoderBeginRenderPass (ClearDepthStencil)");
            WGPU.wgpuRenderPassEncoderEnd(renderPassEncoderImplPtr);
            WGPU.wgpuRenderPassEncoderRelease(renderPassEncoderImplPtr);
        }
    }

    /// <summary>
    /// Finishes recording and returns the command buffer. Consumes the encoder:
    /// it is retired and released here (the returned <see cref="CommandBuffer"/> is
    /// independent), so a later <see cref="Dispose"/> is a no-op and any reuse of
    /// the encoder throws <see cref="ObjectDisposedException"/>.
    /// </summary>
    public unsafe CommandBuffer Finish()
    {
        WGPUCommandEncoderImpl* p = HandleTable<WGPUCommandEncoderImpl>.Retire(_slot, _gen);
        if (p is null) throw new ObjectDisposedException(nameof(CommandEncoder));   // already finished/disposed
        WGPUCommandBufferImpl* cb = WGPU.wgpuCommandEncoderFinish(p, (WGPUCommandBufferDescriptor*)null);
        WGPU.wgpuCommandEncoderRelease(p);                                          // encoder is consumed by Finish
        return (IntPtr)cb != IntPtr.Zero ? new CommandBuffer(cb) : throw new ResourceCreationException("CommandBuffer", "wgpuCommandEncoderFinish");
    }

    public unsafe void PushDebugGroup(scoped ReadOnlySpan<byte> label)
    {
        fixed (byte* data = &label.GetPinnableReference())
            WGPU.wgpuCommandEncoderPushDebugGroup(this.Handle, WgpuDefaults.OptionalStringView(data, label.Length));
    }

    public unsafe void PopDebugGroup() => WGPU.wgpuCommandEncoderPopDebugGroup(this.Handle);

    public unsafe void InsertDebugMarker(scoped ReadOnlySpan<byte> label)
    {
        fixed (byte* data = &label.GetPinnableReference())
            WGPU.wgpuCommandEncoderInsertDebugMarker(this.Handle, WgpuDefaults.OptionalStringView(data, label.Length));
    }

    public unsafe void Dispose()
    {
        WGPUCommandEncoderImpl* p = HandleTable<WGPUCommandEncoderImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuCommandEncoderRelease(p);
    }

    public bool Equals(CommandEncoder other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object obj) => obj is CommandEncoder o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(CommandEncoder left, CommandEncoder right) => left.Equals(right);
    public static bool operator !=(CommandEncoder left, CommandEncoder right) => !left.Equals(right);
}
