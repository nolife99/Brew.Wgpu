namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUCompilationInfo
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("size_t")]
        public nuint messageCount;

        [NativeTypeName("const WGPUCompilationMessage *")]
        public WGPUCompilationMessage* messages;
    }
}
