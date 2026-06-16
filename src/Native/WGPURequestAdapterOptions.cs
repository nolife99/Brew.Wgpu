namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPURequestAdapterOptions
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUFeatureLevel featureLevel;

        public WGPUPowerPreference powerPreference;

        [NativeTypeName("WGPUBool")]
        public uint forceFallbackAdapter;

        public WGPUBackendType backendType;

        [NativeTypeName("WGPUSurface")]
        public WGPUSurfaceImpl* compatibleSurface;
    }
}
