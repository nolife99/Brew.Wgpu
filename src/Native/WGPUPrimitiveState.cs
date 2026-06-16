namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUPrimitiveState
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUPrimitiveTopology topology;

        public WGPUIndexFormat stripIndexFormat;

        public WGPUFrontFace frontFace;

        public WGPUCullMode cullMode;

        [NativeTypeName("WGPUBool")]
        public uint unclippedDepth;
    }
}
