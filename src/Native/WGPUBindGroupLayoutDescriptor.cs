namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUBindGroupLayoutDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("size_t")]
        public nuint entryCount;

        [NativeTypeName("const WGPUBindGroupLayoutEntry *")]
        public WGPUBindGroupLayoutEntry* entries;
    }
}
