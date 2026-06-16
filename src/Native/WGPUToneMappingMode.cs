namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUToneMappingMode : uint
    {
        WGPUToneMappingMode_Standard = 0x00000001,
        WGPUToneMappingMode_Extended = 0x00000002,
        WGPUToneMappingMode_Force32 = 0x7FFFFFFF,
    }
}
