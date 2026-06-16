using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct DepthStencilState
{
    public WGPUTextureFormat Format;
    public bool DepthWriteEnabled;
    public WGPUCompareFunction DepthCompare;
    public uint StencilReadMask;
    public uint StencilWriteMask;
    public int DepthBias;
    public float DepthBiasSlopeScale;
    public float DepthBiasClamp;

    public static DepthStencilState Depth(
      WGPUTextureFormat format,
      WGPUCompareFunction compare = (WGPUCompareFunction)2,
      bool write = true)
    {
        return new DepthStencilState()
        {
            Format = format,
            DepthCompare = compare,
            DepthWriteEnabled = write,
            StencilReadMask = uint.MaxValue,
            StencilWriteMask = uint.MaxValue
        };
    }
}
