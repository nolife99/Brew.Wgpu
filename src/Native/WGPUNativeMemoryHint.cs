namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeMemoryHint : uint
    {
        WGPUNativeMemoryHint_Performance = 0x00000000,
        WGPUNativeMemoryHint_MemoryUsage = 0x00000001,
        WGPUNativeMemoryHint_Manual = 0x00000002,
        WGPUNativeMemoryHint_Force32 = 0x7FFFFFFF,
    }
}
