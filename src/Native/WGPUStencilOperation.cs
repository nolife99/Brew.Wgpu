namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUStencilOperation : uint
    {
        Undefined = 0x00000000,
        Keep = 0x00000001,
        Zero = 0x00000002,
        Replace = 0x00000003,
        Invert = 0x00000004,
        IncrementClamp = 0x00000005,
        DecrementClamp = 0x00000006,
        IncrementWrap = 0x00000007,
        DecrementWrap = 0x00000008,
        Force32 = 0x7FFFFFFF,
    }
}
