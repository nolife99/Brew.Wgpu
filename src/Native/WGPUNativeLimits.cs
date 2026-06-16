namespace Brew.Wgpu.Native
{
    public partial struct WGPUNativeLimits
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("uint32_t")]
        public uint maxNonSamplerBindings;

        [NativeTypeName("uint32_t")]
        public uint maxBindingArrayElementsPerShaderStage;

        [NativeTypeName("uint32_t")]
        public uint maxBindingArraySamplerElementsPerShaderStage;

        [NativeTypeName("uint32_t")]
        public uint maxMultiviewViewCount;
    }
}
