namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUBindGroupEntry
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("uint32_t")]
        public uint binding;

        [NativeTypeName("WGPUBuffer")]
        public WGPUBufferImpl* buffer;

        [NativeTypeName("uint64_t")]
        public ulong offset;

        [NativeTypeName("uint64_t")]
        public ulong size;

        [NativeTypeName("WGPUSampler")]
        public WGPUSamplerImpl* sampler;

        [NativeTypeName("WGPUTextureView")]
        public WGPUTextureViewImpl* textureView;
    }
}
