namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPrimitiveTopology : uint
    {
        Undefined = 0x00000000,
        PointList = 0x00000001,
        LineList = 0x00000002,
        LineStrip = 0x00000003,
        TriangleList = 0x00000004,
        TriangleStrip = 0x00000005,
        Force32 = 0x7FFFFFFF,
    }
}
