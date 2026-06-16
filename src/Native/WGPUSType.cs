namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUSType : uint
    {
        ShaderSourceSPIRV = 0x00000001,
        ShaderSourceWGSL = 0x00000002,
        RenderPassMaxDrawCount = 0x00000003,
        SurfaceSourceMetalLayer = 0x00000004,
        SurfaceSourceWindowsHWND = 0x00000005,
        SurfaceSourceXlibWindow = 0x00000006,
        SurfaceSourceWaylandSurface = 0x00000007,
        SurfaceSourceAndroidNativeWindow = 0x00000008,
        SurfaceSourceXCBWindow = 0x00000009,
        SurfaceColorManagement = 0x0000000A,
        RequestAdapterWebXROptions = 0x0000000B,
        TextureComponentSwizzleDescriptor = 0x0000000C,
        ExternalTextureBindingLayout = 0x0000000D,
        ExternalTextureBindingEntry = 0x0000000E,
        CompatibilityModeLimits = 0x0000000F,
        TextureBindingViewDimension = 0x00000010,
        Force32 = 0x7FFFFFFF,
    }
}
