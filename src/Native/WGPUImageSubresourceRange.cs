namespace Brew.Wgpu.Native
{
    public partial struct WGPUImageSubresourceRange
    {
        public WGPUTextureAspect aspect;

        [NativeTypeName("uint32_t")]
        public uint baseMipLevel;

        [NativeTypeName("uint32_t")]
        public uint mipLevelCount;

        [NativeTypeName("uint32_t")]
        public uint baseArrayLayer;

        [NativeTypeName("uint32_t")]
        public uint arrayLayerCount;
    }
}
