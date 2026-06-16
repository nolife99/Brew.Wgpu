namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUVertexBufferLayout
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUVertexStepMode stepMode;

        [NativeTypeName("uint64_t")]
        public ulong arrayStride;

        [NativeTypeName("size_t")]
        public nuint attributeCount;

        [NativeTypeName("const WGPUVertexAttribute *")]
        public WGPUVertexAttribute* attributes;
    }
}
