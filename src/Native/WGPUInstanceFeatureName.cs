namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUInstanceFeatureName : uint
    {
        WGPUInstanceFeatureName_TimedWaitAny = 0x00000001,
        WGPUInstanceFeatureName_ShaderSourceSPIRV = 0x00000002,
        WGPUInstanceFeatureName_MultipleDevicesPerAdapter = 0x00000003,
        WGPUInstanceFeatureName_Force32 = 0x7FFFFFFF,
    }
}
