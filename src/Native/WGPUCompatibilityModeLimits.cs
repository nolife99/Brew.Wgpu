namespace Brew.Wgpu.Native
{
    public partial struct WGPUCompatibilityModeLimits
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("uint32_t")]
        public uint maxStorageBuffersInVertexStage;

        [NativeTypeName("uint32_t")]
        public uint maxStorageTexturesInVertexStage;

        [NativeTypeName("uint32_t")]
        public uint maxStorageBuffersInFragmentStage;

        [NativeTypeName("uint32_t")]
        public uint maxStorageTexturesInFragmentStage;
    }
}
