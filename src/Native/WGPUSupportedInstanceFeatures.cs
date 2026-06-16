namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUSupportedInstanceFeatures
    {
        [NativeTypeName("size_t")]
        public nuint featureCount;

        [NativeTypeName("const WGPUInstanceFeatureName *")]
        public WGPUInstanceFeatureName* features;
    }
}
