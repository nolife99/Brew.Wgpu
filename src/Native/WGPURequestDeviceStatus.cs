namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPURequestDeviceStatus : uint
    {
        Success = 0x00000001,
        CallbackCancelled = 0x00000002,
        Error = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
