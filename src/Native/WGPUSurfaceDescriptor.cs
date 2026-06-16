namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUSurfaceDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;
    }
}
