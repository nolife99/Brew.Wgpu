namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPURenderBundleDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;
    }
}
