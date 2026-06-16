namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFilterMode : uint
    {
        WGPUFilterMode_Undefined = 0x00000000,
        WGPUFilterMode_Nearest = 0x00000001,
        WGPUFilterMode_Linear = 0x00000002,
        WGPUFilterMode_Force32 = 0x7FFFFFFF,
    }
}
