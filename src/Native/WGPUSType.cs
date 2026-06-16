namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSType : uint
    {
        WGPUSType_ShaderSourceSPIRV = 0x00000001,
        WGPUSType_ShaderSourceWGSL = 0x00000002,
        WGPUSType_RenderPassMaxDrawCount = 0x00000003,
        WGPUSType_SurfaceSourceMetalLayer = 0x00000004,
        WGPUSType_SurfaceSourceWindowsHWND = 0x00000005,
        WGPUSType_SurfaceSourceXlibWindow = 0x00000006,
        WGPUSType_SurfaceSourceWaylandSurface = 0x00000007,
        WGPUSType_SurfaceSourceAndroidNativeWindow = 0x00000008,
        WGPUSType_SurfaceSourceXCBWindow = 0x00000009,
        WGPUSType_SurfaceColorManagement = 0x0000000A,
        WGPUSType_RequestAdapterWebXROptions = 0x0000000B,
        WGPUSType_TextureComponentSwizzleDescriptor = 0x0000000C,
        WGPUSType_ExternalTextureBindingLayout = 0x0000000D,
        WGPUSType_ExternalTextureBindingEntry = 0x0000000E,
        WGPUSType_CompatibilityModeLimits = 0x0000000F,
        WGPUSType_TextureBindingViewDimension = 0x00000010,
        WGPUSType_Force32 = 0x7FFFFFFF,
    }
}
