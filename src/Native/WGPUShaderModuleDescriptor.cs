namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUShaderModuleDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;
    }
}
