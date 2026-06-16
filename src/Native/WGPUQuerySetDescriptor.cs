namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUQuerySetDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        public WGPUQueryType type;

        [NativeTypeName("uint32_t")]
        public uint count;
    }
}
