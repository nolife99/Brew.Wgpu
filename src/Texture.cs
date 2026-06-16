using Brew.Wgpu.Handles;
using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>
/// A GPU texture. The handle is a value type ({slot, generation}); copies are free,
/// disposal releases exactly once and invalidates all copies.
/// </summary>
public readonly struct Texture : IDisposable, IEquatable<Texture>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe Texture(WGPUTextureImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUTextureImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUTextureImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUTextureImpl* Handle
    {
        get
        {
            WGPUTextureImpl* p = HandleTable<WGPUTextureImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(Texture));
            return p;
        }
    }

    public unsafe uint Width => WGPU.wgpuTextureGetWidth(Handle);
    public unsafe uint Height => WGPU.wgpuTextureGetHeight(Handle);
    public unsafe uint DepthOrArrayLayers => WGPU.wgpuTextureGetDepthOrArrayLayers(Handle);
    public unsafe uint MipLevelCount => WGPU.wgpuTextureGetMipLevelCount(Handle);
    public unsafe uint SampleCount => WGPU.wgpuTextureGetSampleCount(Handle);
    public unsafe WGPUTextureFormat Format => WGPU.wgpuTextureGetFormat(Handle);
    public unsafe WGPUTextureDimension Dimension => WGPU.wgpuTextureGetDimension(Handle);
    public unsafe TextureUsage Usage => (TextureUsage)WGPU.wgpuTextureGetUsage(Handle);

    public unsafe TextureView CreateView()
    {
        WGPUTextureViewImpl* view = WGPU.wgpuTextureCreateView(Handle, (WGPUTextureViewDescriptor*)null);
        return (IntPtr)view != IntPtr.Zero
            ? new TextureView(view)
            : throw new ResourceCreationException("TextureView", "wgpuTextureCreateView");
    }

    public unsafe TextureView CreateView(in TextureViewDescriptor desc)
    {
        var native = new WGPUTextureViewDescriptor
        {
            label = new WGPUStringView { data = (sbyte*)null, length = WgpuDefaults.StrLen },
            format = desc.Format,
            dimension = desc.Dimension,
            baseMipLevel = desc.BaseMipLevel,
            mipLevelCount = desc.MipLevelCount == 0U ? uint.MaxValue : desc.MipLevelCount,
            baseArrayLayer = desc.BaseArrayLayer,
            arrayLayerCount = desc.ArrayLayerCount == 0U ? uint.MaxValue : desc.ArrayLayerCount,
            aspect = desc.Aspect,
            usage = (ulong)desc.Usage,
        };
        WGPUTextureViewImpl* view = WGPU.wgpuTextureCreateView(Handle, &native);
        return (IntPtr)view != IntPtr.Zero
            ? new TextureView(view)
            : throw new ResourceCreationException("TextureView", "wgpuTextureCreateView");
    }

    public unsafe void Dispose()
    {
        WGPUTextureImpl* p = HandleTable<WGPUTextureImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuTextureRelease(p);
    }

    public bool Equals(Texture other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is Texture o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(Texture left, Texture right) => left.Equals(right);
    public static bool operator !=(Texture left, Texture right) => !left.Equals(right);
}
