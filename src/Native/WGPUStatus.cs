namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUStatus : uint
    {
        WGPUStatus_Success = 0x00000001,
        WGPUStatus_Error = 0x00000002,
        WGPUStatus_Force32 = 0x7FFFFFFF,
    }
}
