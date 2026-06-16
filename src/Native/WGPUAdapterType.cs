namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUAdapterType : uint
    {
        DiscreteGPU = 0x00000001,
        IntegratedGPU = 0x00000002,
        CPU = 0x00000003,
        Unknown = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
