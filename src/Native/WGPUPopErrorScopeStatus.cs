namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPopErrorScopeStatus : uint
    {
        WGPUPopErrorScopeStatus_Success = 0x00000001,
        WGPUPopErrorScopeStatus_CallbackCancelled = 0x00000002,
        WGPUPopErrorScopeStatus_Error = 0x00000003,
        WGPUPopErrorScopeStatus_Force32 = 0x7FFFFFFF,
    }
}
