namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUTextureAspect : uint
    {
        WGPUTextureAspect_Undefined = 0x00000000,
        WGPUTextureAspect_All = 0x00000001,
        WGPUTextureAspect_StencilOnly = 0x00000002,
        WGPUTextureAspect_DepthOnly = 0x00000003,
        WGPUTextureAspect_Force32 = 0x7FFFFFFF,
    }
}
