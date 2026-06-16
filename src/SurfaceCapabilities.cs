using Brew.Wgpu.Native;
using System;

#nullable enable
namespace Brew.Wgpu;

public readonly struct SurfaceCapabilities
{
    public readonly WGPUTextureFormat[] Formats;
    public readonly WGPUPresentMode[] PresentModes;
    public readonly WGPUCompositeAlphaMode[] AlphaModes;
    public readonly TextureUsage Usages;

    internal SurfaceCapabilities(
      WGPUTextureFormat[] formats,
      WGPUPresentMode[] presentModes,
      WGPUCompositeAlphaMode[] alphaModes,
      TextureUsage usages)
    {
        this.Formats = formats;
        this.PresentModes = presentModes;
        this.AlphaModes = alphaModes;
        this.Usages = usages;
    }

    public bool SupportsFormat(WGPUTextureFormat format)
    {
        return Array.IndexOf<WGPUTextureFormat>(this.Formats, format) >= 0;
    }

    public bool SupportsPresentMode(WGPUPresentMode mode)
    {
        return Array.IndexOf<WGPUPresentMode>(this.PresentModes, mode) >= 0;
    }

    public bool SupportsAlphaMode(WGPUCompositeAlphaMode mode)
    {
        return Array.IndexOf<WGPUCompositeAlphaMode>(this.AlphaModes, mode) >= 0;
    }
}
