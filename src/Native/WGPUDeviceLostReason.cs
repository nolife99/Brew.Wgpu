namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUDeviceLostReason : uint
    {
        Unknown = 0x00000001,
        Destroyed = 0x00000002,
        CallbackCancelled = 0x00000003,
        FailedCreation = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
