namespace Brew.Wgpu.Native
{
    public partial struct WGPUBindGroupLayoutEntryExtras
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("uint32_t")]
        public uint count;
    }
}
