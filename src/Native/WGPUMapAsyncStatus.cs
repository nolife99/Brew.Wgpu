namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUMapAsyncStatus : uint
    {
        Success = 0x00000001,
        CallbackCancelled = 0x00000002,
        Error = 0x00000003,
        Aborted = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
