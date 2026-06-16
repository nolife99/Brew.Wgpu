namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeSurfaceGetCurrentTextureStatus : uint
    {
        WGPUSurfaceGetCurrentTextureStatus_Occluded = 0x00030001,
        WGPUNativeSurfaceGetCurrentTextureStatus_Force32 = 0x7FFFFFFF,
    }
}
