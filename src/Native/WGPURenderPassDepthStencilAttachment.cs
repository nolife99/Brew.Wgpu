namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPURenderPassDepthStencilAttachment
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("WGPUTextureView")]
        public WGPUTextureViewImpl* view;

        public WGPULoadOp depthLoadOp;

        public WGPUStoreOp depthStoreOp;

        public float depthClearValue;

        [NativeTypeName("WGPUBool")]
        public uint depthReadOnly;

        public WGPULoadOp stencilLoadOp;

        public WGPUStoreOp stencilStoreOp;

        [NativeTypeName("uint32_t")]
        public uint stencilClearValue;

        [NativeTypeName("WGPUBool")]
        public uint stencilReadOnly;
    }
}
