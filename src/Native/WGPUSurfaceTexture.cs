namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUSurfaceTexture
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("WGPUTexture")]
        public WGPUTextureImpl* texture;

        public WGPUSurfaceGetCurrentTextureStatus status;
    }
}
