namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUComputePipelineDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("WGPUPipelineLayout")]
        public WGPUPipelineLayoutImpl* layout;

        public WGPUComputeState compute;
    }
}
