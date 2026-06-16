namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUPipelineLayoutDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("size_t")]
        public nuint bindGroupLayoutCount;

        [NativeTypeName("const WGPUBindGroupLayout *")]
        public WGPUBindGroupLayoutImpl** bindGroupLayouts;

        [NativeTypeName("uint32_t")]
        public uint immediateSize;
    }
}
