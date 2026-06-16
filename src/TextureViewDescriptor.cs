using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct TextureViewDescriptor
{
    public WGPUTextureFormat Format;
    public WGPUTextureViewDimension Dimension;
    public uint BaseMipLevel;
    public uint MipLevelCount;
    public uint BaseArrayLayer;
    public uint ArrayLayerCount;
    public WGPUTextureAspect Aspect;
    public TextureUsage Usage;
}
