namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUErrorType : uint
    {
        WGPUErrorType_NoError = 0x00000001,
        WGPUErrorType_Validation = 0x00000002,
        WGPUErrorType_OutOfMemory = 0x00000003,
        WGPUErrorType_Internal = 0x00000004,
        WGPUErrorType_Unknown = 0x00000005,
        WGPUErrorType_Force32 = 0x7FFFFFFF,
    }
}
