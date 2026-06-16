namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPURenderBundleEncoderDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("size_t")]
        public nuint colorFormatCount;

        [NativeTypeName("const WGPUTextureFormat *")]
        public WGPUTextureFormat* colorFormats;

        public WGPUTextureFormat depthStencilFormat;

        [NativeTypeName("uint32_t")]
        public uint sampleCount;

        [NativeTypeName("WGPUBool")]
        public uint depthReadOnly;

        [NativeTypeName("WGPUBool")]
        public uint stencilReadOnly;
    }
}
