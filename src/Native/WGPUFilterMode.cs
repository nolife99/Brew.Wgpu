namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFilterMode : uint
    {
        Undefined = 0x00000000,
        Nearest = 0x00000001,
        Linear = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
