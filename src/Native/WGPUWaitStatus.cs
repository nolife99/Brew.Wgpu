namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUWaitStatus : uint
    {
        WGPUWaitStatus_Success = 0x00000001,
        WGPUWaitStatus_TimedOut = 0x00000002,
        WGPUWaitStatus_Error = 0x00000003,
        WGPUWaitStatus_Force32 = 0x7FFFFFFF,
    }
}
