namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUTextureDimension : uint
    {
        WGPUTextureDimension_Undefined = 0x00000000,
        WGPUTextureDimension_1D = 0x00000001,
        WGPUTextureDimension_2D = 0x00000002,
        WGPUTextureDimension_3D = 0x00000003,
        WGPUTextureDimension_Force32 = 0x7FFFFFFF,
    }
}
