namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCreatePipelineAsyncStatus : uint
    {
        Success = 0x00000001,
        CallbackCancelled = 0x00000002,
        ValidationError = 0x00000003,
        InternalError = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
