namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeQueryType : uint
    {
        PipelineStatistics = 0x00030000,
        Force32 = 0x7FFFFFFF,
    }
}
