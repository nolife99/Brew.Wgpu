namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUComputePassDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("const WGPUPassTimestampWrites *")]
        public WGPUPassTimestampWrites* timestampWrites;
    }
}
