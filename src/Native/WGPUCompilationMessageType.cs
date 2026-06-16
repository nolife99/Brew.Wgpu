namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCompilationMessageType : uint
    {
        Error = 0x00000001,
        Warning = 0x00000002,
        Info = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
