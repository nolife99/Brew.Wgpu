namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPURenderPassDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("size_t")]
        public nuint colorAttachmentCount;

        [NativeTypeName("const WGPURenderPassColorAttachment *")]
        public WGPURenderPassColorAttachment* colorAttachments;

        [NativeTypeName("const WGPURenderPassDepthStencilAttachment *")]
        public WGPURenderPassDepthStencilAttachment* depthStencilAttachment;

        [NativeTypeName("WGPUQuerySet")]
        public WGPUQuerySetImpl* occlusionQuerySet;

        [NativeTypeName("const WGPUPassTimestampWrites *")]
        public WGPUPassTimestampWrites* timestampWrites;
    }
}
