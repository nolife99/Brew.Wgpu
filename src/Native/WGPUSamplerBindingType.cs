namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSamplerBindingType : uint
    {
        WGPUSamplerBindingType_BindingNotUsed = 0x00000000,
        WGPUSamplerBindingType_Undefined = 0x00000001,
        WGPUSamplerBindingType_Filtering = 0x00000002,
        WGPUSamplerBindingType_NonFiltering = 0x00000003,
        WGPUSamplerBindingType_Comparison = 0x00000004,
        WGPUSamplerBindingType_Force32 = 0x7FFFFFFF,
    }
}
