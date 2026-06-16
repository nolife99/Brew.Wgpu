namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSamplerBorderColor : uint
    {
        Undefined = 0x00000000,
        TransparentBlack = 0x00000001,
        OpaqueBlack = 0x00000002,
        OpaqueWhite = 0x00000003,
        Zero = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
