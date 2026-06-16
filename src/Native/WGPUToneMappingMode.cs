namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUToneMappingMode : uint
    {
        Standard = 0x00000001,
        Extended = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
