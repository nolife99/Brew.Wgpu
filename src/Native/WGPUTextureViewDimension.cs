namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUTextureViewDimension : uint
    {
        WGPUTextureViewDimension_Undefined = 0x00000000,
        WGPUTextureViewDimension_1D = 0x00000001,
        WGPUTextureViewDimension_2D = 0x00000002,
        WGPUTextureViewDimension_2DArray = 0x00000003,
        WGPUTextureViewDimension_Cube = 0x00000004,
        WGPUTextureViewDimension_CubeArray = 0x00000005,
        WGPUTextureViewDimension_3D = 0x00000006,
        WGPUTextureViewDimension_Force32 = 0x7FFFFFFF,
    }
}
