namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUCommandBufferDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;
    }
}
