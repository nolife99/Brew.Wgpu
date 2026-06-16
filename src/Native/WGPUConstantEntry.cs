namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUConstantEntry
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView key;

        public double value;
    }
}
