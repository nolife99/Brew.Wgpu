namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUTextureSampleType : uint
    {
        BindingNotUsed = 0x00000000,
        Undefined = 0x00000001,
        Float = 0x00000002,
        UnfilterableFloat = 0x00000003,
        Depth = 0x00000004,
        Sint = 0x00000005,
        Uint = 0x00000006,
        Force32 = 0x7FFFFFFF,
    }
}
