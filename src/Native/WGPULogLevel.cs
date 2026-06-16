namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPULogLevel : uint
    {
        WGPULogLevel_Off = 0x00000000,
        WGPULogLevel_Error = 0x00000001,
        WGPULogLevel_Warn = 0x00000002,
        WGPULogLevel_Info = 0x00000003,
        WGPULogLevel_Debug = 0x00000004,
        WGPULogLevel_Trace = 0x00000005,
        WGPULogLevel_Force32 = 0x7FFFFFFF,
    }
}
