namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUTextureViewDimension : uint
    {
        Undefined = 0x00000000,
        _1D = 0x00000001,
        _2D = 0x00000002,
        _2DArray = 0x00000003,
        Cube = 0x00000004,
        CubeArray = 0x00000005,
        _3D = 0x00000006,
        Force32 = 0x7FFFFFFF,
    }
}
