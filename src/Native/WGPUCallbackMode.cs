namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCallbackMode : uint
    {
        WaitAnyOnly = 0x00000001,
        AllowProcessEvents = 0x00000002,
        AllowSpontaneous = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
