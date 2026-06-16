namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUCompilationMessage
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView message;

        public WGPUCompilationMessageType type;

        [NativeTypeName("uint64_t")]
        public ulong lineNum;

        [NativeTypeName("uint64_t")]
        public ulong linePos;

        [NativeTypeName("uint64_t")]
        public ulong offset;

        [NativeTypeName("uint64_t")]
        public ulong length;
    }
}
