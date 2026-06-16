namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUStatus : uint
    {
        Success = 0x00000001,
        Error = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
