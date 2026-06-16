using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct SamplerDescriptor
{
    public WGPUAddressMode AddressModeU;
    public WGPUAddressMode AddressModeV;
    public WGPUAddressMode AddressModeW;
    public WGPUFilterMode MagFilter;
    public WGPUFilterMode MinFilter;
    public WGPUMipmapFilterMode MipmapFilter;
    public float LodMinClamp;
    public float LodMaxClamp;
    public WGPUCompareFunction Compare;
    public ushort MaxAnisotropy;
}
