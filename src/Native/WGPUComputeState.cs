namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUComputeState
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("WGPUShaderModule")]
        public WGPUShaderModuleImpl* module;

        public WGPUStringView entryPoint;

        [NativeTypeName("size_t")]
        public nuint constantCount;

        [NativeTypeName("const WGPUConstantEntry *")]
        public WGPUConstantEntry* constants;
    }
}
