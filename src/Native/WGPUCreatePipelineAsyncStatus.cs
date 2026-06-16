namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCreatePipelineAsyncStatus : uint
    {
        WGPUCreatePipelineAsyncStatus_Success = 0x00000001,
        WGPUCreatePipelineAsyncStatus_CallbackCancelled = 0x00000002,
        WGPUCreatePipelineAsyncStatus_ValidationError = 0x00000003,
        WGPUCreatePipelineAsyncStatus_InternalError = 0x00000004,
        WGPUCreatePipelineAsyncStatus_Force32 = 0x7FFFFFFF,
    }
}
