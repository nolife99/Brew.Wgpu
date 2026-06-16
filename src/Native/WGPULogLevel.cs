namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPULogLevel : uint
    {
        Off = 0x00000000,
        Error = 0x00000001,
        Warn = 0x00000002,
        Info = 0x00000003,
        Debug = 0x00000004,
        Trace = 0x00000005,
        Force32 = 0x7FFFFFFF,
    }
}
