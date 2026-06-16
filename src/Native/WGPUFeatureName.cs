namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFeatureName : uint
    {
        CoreFeaturesAndLimits = 0x00000001,
        DepthClipControl = 0x00000002,
        Depth32FloatStencil8 = 0x00000003,
        TextureCompressionBC = 0x00000004,
        TextureCompressionBCSliced3D = 0x00000005,
        TextureCompressionETC2 = 0x00000006,
        TextureCompressionASTC = 0x00000007,
        TextureCompressionASTCSliced3D = 0x00000008,
        TimestampQuery = 0x00000009,
        IndirectFirstInstance = 0x0000000A,
        ShaderF16 = 0x0000000B,
        RG11B10UfloatRenderable = 0x0000000C,
        BGRA8UnormStorage = 0x0000000D,
        Float32Filterable = 0x0000000E,
        Float32Blendable = 0x0000000F,
        ClipDistances = 0x00000010,
        DualSourceBlending = 0x00000011,
        Subgroups = 0x00000012,
        TextureFormatsTier1 = 0x00000013,
        TextureFormatsTier2 = 0x00000014,
        PrimitiveIndex = 0x00000015,
        TextureComponentSwizzle = 0x00000016,
        Force32 = 0x7FFFFFFF,
    }
}
