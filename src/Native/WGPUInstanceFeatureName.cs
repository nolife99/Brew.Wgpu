namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUInstanceFeatureName : uint
    {
        TimedWaitAny = 0x00000001,
        ShaderSourceSPIRV = 0x00000002,
        MultipleDevicesPerAdapter = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
