namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUQueueDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;
    }
}
