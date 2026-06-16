namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUVertexAttribute
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUVertexFormat format;

        [NativeTypeName("uint64_t")]
        public ulong offset;

        [NativeTypeName("uint32_t")]
        public uint shaderLocation;
    }
}
