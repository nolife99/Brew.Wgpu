namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUCommandEncoderDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;
    }
}
