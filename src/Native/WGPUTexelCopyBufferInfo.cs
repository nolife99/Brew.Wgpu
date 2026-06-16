namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUTexelCopyBufferInfo
    {
        public WGPUTexelCopyBufferLayout layout;

        [NativeTypeName("WGPUBuffer")]
        public WGPUBufferImpl* buffer;
    }
}
