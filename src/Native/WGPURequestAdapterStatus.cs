namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPURequestAdapterStatus : uint
    {
        WGPURequestAdapterStatus_Success = 0x00000001,
        WGPURequestAdapterStatus_CallbackCancelled = 0x00000002,
        WGPURequestAdapterStatus_Unavailable = 0x00000003,
        WGPURequestAdapterStatus_Error = 0x00000004,
        WGPURequestAdapterStatus_Force32 = 0x7FFFFFFF,
    }
}
