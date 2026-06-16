namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUAddressMode : uint
    {
        Undefined = 0x00000000,
        ClampToEdge = 0x00000001,
        Repeat = 0x00000002,
        MirrorRepeat = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
