namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUAddressMode : uint
    {
        WGPUAddressMode_Undefined = 0x00000000,
        WGPUAddressMode_ClampToEdge = 0x00000001,
        WGPUAddressMode_Repeat = 0x00000002,
        WGPUAddressMode_MirrorRepeat = 0x00000003,
        WGPUAddressMode_Force32 = 0x7FFFFFFF,
    }
}
