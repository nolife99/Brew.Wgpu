namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUMapAsyncStatus : uint
    {
        WGPUMapAsyncStatus_Success = 0x00000001,
        WGPUMapAsyncStatus_CallbackCancelled = 0x00000002,
        WGPUMapAsyncStatus_Error = 0x00000003,
        WGPUMapAsyncStatus_Aborted = 0x00000004,
        WGPUMapAsyncStatus_Force32 = 0x7FFFFFFF,
    }
}
