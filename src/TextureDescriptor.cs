using Brew.Wgpu.Native;
using System;

#nullable disable
namespace Brew.Wgpu;

public ref struct TextureDescriptor
{
    public WGPUExtent3D Size;
    public WGPUTextureFormat Format;
    public TextureUsage Usage;
    public WGPUTextureDimension Dimension;
    public uint MipLevelCount;
    public uint SampleCount;
    public ReadOnlySpan<WGPUTextureFormat> ViewFormats;
}
