namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSurfaceGetCurrentTextureStatus : uint
    {
        WGPUSurfaceGetCurrentTextureStatus_SuccessOptimal = 0x00000001,
        WGPUSurfaceGetCurrentTextureStatus_SuccessSuboptimal = 0x00000002,
        WGPUSurfaceGetCurrentTextureStatus_Timeout = 0x00000003,
        WGPUSurfaceGetCurrentTextureStatus_Outdated = 0x00000004,
        WGPUSurfaceGetCurrentTextureStatus_Lost = 0x00000005,
        WGPUSurfaceGetCurrentTextureStatus_Error = 0x00000006,
        WGPUSurfaceGetCurrentTextureStatus_Force32 = 0x7FFFFFFF,
    }
}
