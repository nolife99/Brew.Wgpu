namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUMipmapFilterMode : uint
    {
        WGPUMipmapFilterMode_Undefined = 0x00000000,
        WGPUMipmapFilterMode_Nearest = 0x00000001,
        WGPUMipmapFilterMode_Linear = 0x00000002,
        WGPUMipmapFilterMode_Force32 = 0x7FFFFFFF,
    }
}
