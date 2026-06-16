namespace Brew.Wgpu.Native
{
    public partial struct WGPUTexelCopyBufferLayout
    {
        [NativeTypeName("uint64_t")]
        public ulong offset;

        [NativeTypeName("uint32_t")]
        public uint bytesPerRow;

        [NativeTypeName("uint32_t")]
        public uint rowsPerImage;
    }
}
