namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUTextureAspect : uint
    {
        Undefined = 0x00000000,
        All = 0x00000001,
        StencilOnly = 0x00000002,
        DepthOnly = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
