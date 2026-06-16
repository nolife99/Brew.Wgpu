namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeTextureFormat : uint
    {
        WGPUNativeTextureFormat_R16Unorm = 0x00030001,
        WGPUNativeTextureFormat_R16Snorm = 0x00030002,
        WGPUNativeTextureFormat_Rg16Unorm = 0x00030003,
        WGPUNativeTextureFormat_Rg16Snorm = 0x00030004,
        WGPUNativeTextureFormat_Rgba16Unorm = 0x00030005,
        WGPUNativeTextureFormat_Rgba16Snorm = 0x00030006,
        WGPUNativeTextureFormat_NV12 = 0x00030007,
        WGPUNativeTextureFormat_P010 = 0x00030008,
    }
}
