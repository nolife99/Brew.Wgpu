namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeMemoryHint : uint
    {
        Performance = 0x00000000,
        MemoryUsage = 0x00000001,
        Manual = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
