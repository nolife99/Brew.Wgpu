namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSamplerBorderColor : uint
    {
        WGPUSamplerBorderColor_Undefined = 0x00000000,
        WGPUSamplerBorderColor_TransparentBlack = 0x00000001,
        WGPUSamplerBorderColor_OpaqueBlack = 0x00000002,
        WGPUSamplerBorderColor_OpaqueWhite = 0x00000003,
        WGPUSamplerBorderColor_Zero = 0x00000004,
        WGPUSamplerBorderColor_Force32 = 0x7FFFFFFF,
    }
}
