namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUWGSLLanguageFeatureName : uint
    {
        ReadonlyAndReadwriteStorageTextures = 0x00000001,
        Packed4x8IntegerDotProduct = 0x00000002,
        UnrestrictedPointerParameters = 0x00000003,
        PointerCompositeAccess = 0x00000004,
        UniformBufferStandardLayout = 0x00000005,
        SubgroupId = 0x00000006,
        TextureAndSamplerLet = 0x00000007,
        SubgroupUniformity = 0x00000008,
        TextureFormatsTier1 = 0x00000009,
        Force32 = 0x7FFFFFFF,
    }
}
