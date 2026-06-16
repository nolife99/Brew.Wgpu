namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUDeviceLostReason : uint
    {
        WGPUDeviceLostReason_Unknown = 0x00000001,
        WGPUDeviceLostReason_Destroyed = 0x00000002,
        WGPUDeviceLostReason_CallbackCancelled = 0x00000003,
        WGPUDeviceLostReason_FailedCreation = 0x00000004,
        WGPUDeviceLostReason_Force32 = 0x7FFFFFFF,
    }
}
