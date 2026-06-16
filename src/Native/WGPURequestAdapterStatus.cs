namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPURequestAdapterStatus : uint
    {
        Success = 0x00000001,
        CallbackCancelled = 0x00000002,
        Unavailable = 0x00000003,
        Error = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
