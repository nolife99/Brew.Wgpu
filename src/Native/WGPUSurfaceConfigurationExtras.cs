namespace Brew.Wgpu.Native
{
    public partial struct WGPUSurfaceConfigurationExtras
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("uint32_t")]
        public uint desiredMaximumFrameLatency;
    }
}
