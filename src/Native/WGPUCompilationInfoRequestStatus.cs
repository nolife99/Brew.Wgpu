namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCompilationInfoRequestStatus : uint
    {
        Success = 0x00000001,
        CallbackCancelled = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
