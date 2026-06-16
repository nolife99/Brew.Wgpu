namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUQuerySetDescriptorExtras
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("const WGPUPipelineStatisticName *")]
        public WGPUPipelineStatisticName* pipelineStatistics;

        [NativeTypeName("size_t")]
        public nuint pipelineStatisticCount;
    }
}
