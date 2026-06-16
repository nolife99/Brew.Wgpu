namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPowerPreference : uint
    {
        Undefined = 0x00000000,
        LowPower = 0x00000001,
        HighPerformance = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
