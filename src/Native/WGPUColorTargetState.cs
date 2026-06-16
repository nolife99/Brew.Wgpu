namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUColorTargetState
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUTextureFormat format;

        [NativeTypeName("const WGPUBlendState *")]
        public WGPUBlendState* blend;

        [NativeTypeName("WGPUColorWriteMask")]
        public ulong writeMask;
    }
}
