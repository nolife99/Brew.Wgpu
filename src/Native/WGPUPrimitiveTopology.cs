namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPrimitiveTopology : uint
    {
        WGPUPrimitiveTopology_Undefined = 0x00000000,
        WGPUPrimitiveTopology_PointList = 0x00000001,
        WGPUPrimitiveTopology_LineList = 0x00000002,
        WGPUPrimitiveTopology_LineStrip = 0x00000003,
        WGPUPrimitiveTopology_TriangleList = 0x00000004,
        WGPUPrimitiveTopology_TriangleStrip = 0x00000005,
        WGPUPrimitiveTopology_Force32 = 0x7FFFFFFF,
    }
}
