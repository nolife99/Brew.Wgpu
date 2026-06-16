using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>
/// A presentable surface. The handle is a value type ({slot, generation}); copies are
/// free, disposal releases exactly once and invalidates all copies.
/// </summary>
public readonly struct Surface : IDisposable, IEquatable<Surface>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe Surface(WGPUSurfaceImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUSurfaceImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUSurfaceImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUSurfaceImpl* Handle
    {
        get
        {
            WGPUSurfaceImpl* p = HandleTable<WGPUSurfaceImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(Surface));
            return p;
        }
    }

    public unsafe WGPUTextureFormat GetPreferredFormat(Adapter adapter)
    {
        var caps = new WGPUSurfaceCapabilities();
        WGPU.wgpuSurfaceGetCapabilities(Handle, adapter.Handle, &caps);
        int preferred = caps.formatCount > UIntPtr.Zero ? *(int*)caps.formats : 27;
        WGPU.wgpuSurfaceCapabilitiesFreeMembers(caps);
        return (WGPUTextureFormat)preferred;
    }

    public unsafe SurfaceCapabilities GetCapabilities(Adapter adapter)
    {
        var caps = new WGPUSurfaceCapabilities();
        WGPU.wgpuSurfaceGetCapabilities(Handle, adapter.Handle, &caps);
        try
        {
            var formats = new WGPUTextureFormat[(int)caps.formatCount];
            for (int i = 0; i < formats.Length; ++i)
                formats[i] = (WGPUTextureFormat)(((int*)caps.formats)[i]);
            var presentModes = new WGPUPresentMode[(int)caps.presentModeCount];
            for (int i = 0; i < presentModes.Length; ++i)
                presentModes[i] = (WGPUPresentMode)(((int*)caps.presentModes)[i]);
            var alphaModes = new WGPUCompositeAlphaMode[(int)caps.alphaModeCount];
            for (int i = 0; i < alphaModes.Length; ++i)
                alphaModes[i] = (WGPUCompositeAlphaMode)(((int*)caps.alphaModes)[i]);
            return new SurfaceCapabilities(formats, presentModes, alphaModes, (TextureUsage)caps.usages);
        }
        finally
        {
            WGPU.wgpuSurfaceCapabilitiesFreeMembers(caps);
        }
    }

    public unsafe void Configure(
        Device device,
        WGPUTextureFormat format,
        TextureUsage usage,
        uint width,
        uint height,
        WGPUPresentMode presentMode = WGPUPresentMode.Fifo,
        WGPUCompositeAlphaMode alphaMode = WGPUCompositeAlphaMode.Auto)
    {
        var config = new WGPUSurfaceConfiguration
        {
            device = device.Handle,
            format = format,
            usage = (ulong)usage,
            width = width,
            height = height,
            presentMode = presentMode,
            alphaMode = alphaMode,
        };
        WGPU.wgpuSurfaceConfigure(Handle, &config);
    }

    public unsafe SurfaceAcquireResult GetCurrentTexture()
    {
        var surfaceTexture = new WGPUSurfaceTexture();
        WGPU.wgpuSurfaceGetCurrentTexture(Handle, &surfaceTexture);
        return new SurfaceAcquireResult(new Texture(surfaceTexture.texture), surfaceTexture.status);
    }

    public unsafe void Present() => WGPU.wgpuSurfacePresent(Handle);

    public unsafe void Dispose()
    {
        WGPUSurfaceImpl* p = HandleTable<WGPUSurfaceImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuSurfaceRelease(p);
    }

    public bool Equals(Surface other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is Surface o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(Surface left, Surface right) => left.Equals(right);
    public static bool operator !=(Surface left, Surface right) => !left.Equals(right);
}
