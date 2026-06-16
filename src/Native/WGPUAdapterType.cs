namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUAdapterType : uint
    {
        WGPUAdapterType_DiscreteGPU = 0x00000001,
        WGPUAdapterType_IntegratedGPU = 0x00000002,
        WGPUAdapterType_CPU = 0x00000003,
        WGPUAdapterType_Unknown = 0x00000004,
        WGPUAdapterType_Force32 = 0x7FFFFFFF,
    }
}
