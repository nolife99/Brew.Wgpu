namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUBindGroupLayoutEntry
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("uint32_t")]
        public uint binding;

        [NativeTypeName("WGPUShaderStage")]
        public ulong visibility;

        [NativeTypeName("uint32_t")]
        public uint bindingArraySize;

        public WGPUBufferBindingLayout buffer;

        public WGPUSamplerBindingLayout sampler;

        public WGPUTextureBindingLayout texture;

        public WGPUStorageTextureBindingLayout storageTexture;
    }
}
