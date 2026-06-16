namespace Brew.Wgpu.Native
{
    public partial struct WGPUPrimitiveStateExtras
    {
        public WGPUChainedStruct chain;

        public WGPUPolygonMode polygonMode;

        [NativeTypeName("WGPUBool")]
        public uint conservative;
    }
}
