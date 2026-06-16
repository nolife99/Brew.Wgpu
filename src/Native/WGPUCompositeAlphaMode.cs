namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCompositeAlphaMode : uint
    {
        Auto = 0x00000000,
        Opaque = 0x00000001,
        Premultiplied = 0x00000002,
        Unpremultiplied = 0x00000003,
        Inherit = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
