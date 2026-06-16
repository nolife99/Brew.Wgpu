namespace Brew.Wgpu.Native
{
    public partial struct WGPURequestAdapterWebXROptions
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("WGPUBool")]
        public uint xrCompatible;
    }
}
