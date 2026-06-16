using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct PrimitiveState
{
    public WGPUPrimitiveTopology Topology;
    public WGPUIndexFormat StripIndexFormat;
    public WGPUFrontFace FrontFace;
    public WGPUCullMode CullMode;
    public bool UnclippedDepth;

    public static PrimitiveState Default
    {
        get
        {
            return new PrimitiveState()
            {
                Topology = WGPUPrimitiveTopology.TriangleList,
                FrontFace = WGPUFrontFace.CCW,
                CullMode = WGPUCullMode.None
            };
        }
    }
}
