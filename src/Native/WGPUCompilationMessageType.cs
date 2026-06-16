namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCompilationMessageType : uint
    {
        WGPUCompilationMessageType_Error = 0x00000001,
        WGPUCompilationMessageType_Warning = 0x00000002,
        WGPUCompilationMessageType_Info = 0x00000003,
        WGPUCompilationMessageType_Force32 = 0x7FFFFFFF,
    }
}
