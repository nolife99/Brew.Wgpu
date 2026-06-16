namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUSupportedFeatures
    {
        [NativeTypeName("size_t")]
        public nuint featureCount;

        [NativeTypeName("const WGPUFeatureName *")]
        public WGPUFeatureName* features;
    }
}
