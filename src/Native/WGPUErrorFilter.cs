namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUErrorFilter : uint
    {
        WGPUErrorFilter_Validation = 0x00000001,
        WGPUErrorFilter_OutOfMemory = 0x00000002,
        WGPUErrorFilter_Internal = 0x00000003,
        WGPUErrorFilter_Force32 = 0x7FFFFFFF,
    }
}
