namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUMultisampleState
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("uint32_t")]
        public uint count;

        [NativeTypeName("uint32_t")]
        public uint mask;

        [NativeTypeName("WGPUBool")]
        public uint alphaToCoverageEnabled;
    }
}
