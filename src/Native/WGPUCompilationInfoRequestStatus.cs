namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCompilationInfoRequestStatus : uint
    {
        WGPUCompilationInfoRequestStatus_Success = 0x00000001,
        WGPUCompilationInfoRequestStatus_CallbackCancelled = 0x00000002,
        WGPUCompilationInfoRequestStatus_Force32 = 0x7FFFFFFF,
    }
}
