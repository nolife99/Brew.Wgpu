namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeTextureFormat : uint
    {
        R16Unorm = 0x00030001,
        R16Snorm = 0x00030002,
        Rg16Unorm = 0x00030003,
        Rg16Snorm = 0x00030004,
        Rgba16Unorm = 0x00030005,
        Rgba16Snorm = 0x00030006,
        NV12 = 0x00030007,
        P010 = 0x00030008,
    }
}
