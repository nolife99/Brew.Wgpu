using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct ColorTargetState(WGPUTextureFormat format)
{
    public WGPUTextureFormat Format = format;
    public WGPUBlendState? Blend = new WGPUBlendState?();
    public ColorWriteMask WriteMask = ColorWriteMask.All;
}
