namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUTextureBindingLayout
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUTextureSampleType sampleType;

        public WGPUTextureViewDimension viewDimension;

        [NativeTypeName("WGPUBool")]
        public uint multisampled;
    }
}
