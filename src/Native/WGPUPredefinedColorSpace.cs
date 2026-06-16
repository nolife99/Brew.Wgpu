namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPredefinedColorSpace : uint
    {
        SRGB = 0x00000001,
        DisplayP3 = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
