namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeSType : uint
    {
        WGPUSType_DeviceExtras = 0x00030001,
        WGPUSType_NativeLimits = 0x00030002,
        WGPUSType_ShaderSourceGLSL = 0x00030003,
        WGPUSType_InstanceExtras = 0x00030004,
        WGPUSType_BindGroupEntryExtras = 0x00030005,
        WGPUSType_BindGroupLayoutEntryExtras = 0x00030006,
        WGPUSType_QuerySetDescriptorExtras = 0x00030007,
        WGPUSType_SurfaceConfigurationExtras = 0x00030008,
        WGPUSType_SurfaceSourceSwapChainPanel = 0x00030009,
        WGPUSType_PrimitiveStateExtras = 0x0003000A,
        WGPUSType_SamplerDescriptorExtras = 0x0003000B,
        Force32 = 0x7FFFFFFF,
    }
}
