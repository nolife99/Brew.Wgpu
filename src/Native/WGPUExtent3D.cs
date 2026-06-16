namespace Brew.Wgpu.Native
{
    public partial struct WGPUExtent3D
    {
        [NativeTypeName("uint32_t")]
        public uint width;

        [NativeTypeName("uint32_t")]
        public uint height;

        [NativeTypeName("uint32_t")]
        public uint depthOrArrayLayers;
    }
}
