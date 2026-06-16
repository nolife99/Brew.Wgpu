namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeQueryType : uint
    {
        WGPUNativeQueryType_PipelineStatistics = 0x00030000,
        WGPUNativeQueryType_Force32 = 0x7FFFFFFF,
    }
}
