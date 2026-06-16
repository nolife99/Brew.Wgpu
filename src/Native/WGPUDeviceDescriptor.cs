namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUDeviceDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("size_t")]
        public nuint requiredFeatureCount;

        [NativeTypeName("const WGPUFeatureName *")]
        public WGPUFeatureName* requiredFeatures;

        [NativeTypeName("const WGPULimits *")]
        public WGPULimits* requiredLimits;

        public WGPUQueueDescriptor defaultQueue;

        public WGPUDeviceLostCallbackInfo deviceLostCallbackInfo;

        public WGPUUncapturedErrorCallbackInfo uncapturedErrorCallbackInfo;
    }
}
