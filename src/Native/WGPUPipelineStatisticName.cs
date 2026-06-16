namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPipelineStatisticName : uint
    {
        VertexShaderInvocations = 0x00000000,
        ClipperInvocations = 0x00000001,
        ClipperPrimitivesOut = 0x00000002,
        FragmentShaderInvocations = 0x00000003,
        ComputeShaderInvocations = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
