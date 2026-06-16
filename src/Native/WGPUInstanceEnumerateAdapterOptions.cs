namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUInstanceEnumerateAdapterOptions
    {
        [NativeTypeName("const WGPUChainedStruct *")]
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("WGPUInstanceBackend")]
        public ulong backends;
    }
}
