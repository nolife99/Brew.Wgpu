namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUQueueWorkDoneStatus : uint
    {
        WGPUQueueWorkDoneStatus_Success = 0x00000001,
        WGPUQueueWorkDoneStatus_CallbackCancelled = 0x00000002,
        WGPUQueueWorkDoneStatus_Error = 0x00000003,
        WGPUQueueWorkDoneStatus_Force32 = 0x7FFFFFFF,
    }
}
