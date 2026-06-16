namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSurfaceGetCurrentTextureStatus : uint
    {
        SuccessOptimal = 0x00000001,
        SuccessSuboptimal = 0x00000002,
        Timeout = 0x00000003,
        Outdated = 0x00000004,
        Lost = 0x00000005,
        Error = 0x00000006,
        Force32 = 0x7FFFFFFF,
    }
}
