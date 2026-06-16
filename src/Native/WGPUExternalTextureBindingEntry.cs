namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUExternalTextureBindingEntry
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("WGPUExternalTexture")]
        public WGPUExternalTextureImpl* externalTexture;
    }
}
