namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSamplerBindingType : uint
    {
        BindingNotUsed = 0x00000000,
        Undefined = 0x00000001,
        Filtering = 0x00000002,
        NonFiltering = 0x00000003,
        Comparison = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
