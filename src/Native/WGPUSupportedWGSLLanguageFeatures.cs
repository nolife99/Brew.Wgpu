namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUSupportedWGSLLanguageFeatures
    {
        [NativeTypeName("size_t")]
        public nuint featureCount;

        [NativeTypeName("const WGPUWGSLLanguageFeatureName *")]
        public WGPUWGSLLanguageFeatureName* features;
    }
}
