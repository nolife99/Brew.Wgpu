namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPredefinedColorSpace : uint
    {
        WGPUPredefinedColorSpace_SRGB = 0x00000001,
        WGPUPredefinedColorSpace_DisplayP3 = 0x00000002,
        WGPUPredefinedColorSpace_Force32 = 0x7FFFFFFF,
    }
}
