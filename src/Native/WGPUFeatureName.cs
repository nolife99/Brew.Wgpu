namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFeatureName : uint
    {
        WGPUFeatureName_CoreFeaturesAndLimits = 0x00000001,
        WGPUFeatureName_DepthClipControl = 0x00000002,
        WGPUFeatureName_Depth32FloatStencil8 = 0x00000003,
        WGPUFeatureName_TextureCompressionBC = 0x00000004,
        WGPUFeatureName_TextureCompressionBCSliced3D = 0x00000005,
        WGPUFeatureName_TextureCompressionETC2 = 0x00000006,
        WGPUFeatureName_TextureCompressionASTC = 0x00000007,
        WGPUFeatureName_TextureCompressionASTCSliced3D = 0x00000008,
        WGPUFeatureName_TimestampQuery = 0x00000009,
        WGPUFeatureName_IndirectFirstInstance = 0x0000000A,
        WGPUFeatureName_ShaderF16 = 0x0000000B,
        WGPUFeatureName_RG11B10UfloatRenderable = 0x0000000C,
        WGPUFeatureName_BGRA8UnormStorage = 0x0000000D,
        WGPUFeatureName_Float32Filterable = 0x0000000E,
        WGPUFeatureName_Float32Blendable = 0x0000000F,
        WGPUFeatureName_ClipDistances = 0x00000010,
        WGPUFeatureName_DualSourceBlending = 0x00000011,
        WGPUFeatureName_Subgroups = 0x00000012,
        WGPUFeatureName_TextureFormatsTier1 = 0x00000013,
        WGPUFeatureName_TextureFormatsTier2 = 0x00000014,
        WGPUFeatureName_PrimitiveIndex = 0x00000015,
        WGPUFeatureName_TextureComponentSwizzle = 0x00000016,
        WGPUFeatureName_Force32 = 0x7FFFFFFF,
    }
}
