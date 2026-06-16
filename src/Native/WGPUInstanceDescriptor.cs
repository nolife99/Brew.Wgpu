namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUInstanceDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("size_t")]
        public nuint requiredFeatureCount;

        [NativeTypeName("const WGPUInstanceFeatureName *")]
        public WGPUInstanceFeatureName* requiredFeatures;

        [NativeTypeName("const WGPUInstanceLimits *")]
        public WGPUInstanceLimits* requiredLimits;
    }
}
