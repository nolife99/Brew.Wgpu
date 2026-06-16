namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPURenderPipelineDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("WGPUPipelineLayout")]
        public WGPUPipelineLayoutImpl* layout;

        public WGPUVertexState vertex;

        public WGPUPrimitiveState primitive;

        [NativeTypeName("const WGPUDepthStencilState *")]
        public WGPUDepthStencilState* depthStencil;

        public WGPUMultisampleState multisample;

        [NativeTypeName("const WGPUFragmentState *")]
        public WGPUFragmentState* fragment;
    }
}
