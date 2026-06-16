namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFeatureLevel : uint
    {
        WGPUFeatureLevel_Undefined = 0x00000000,
        WGPUFeatureLevel_Compatibility = 0x00000001,
        WGPUFeatureLevel_Core = 0x00000002,
        WGPUFeatureLevel_Force32 = 0x7FFFFFFF,
    }
}
