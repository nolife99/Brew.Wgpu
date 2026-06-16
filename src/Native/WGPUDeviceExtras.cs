namespace Brew.Wgpu.Native
{
    public partial struct WGPUDeviceExtras
    {
        public WGPUChainedStruct chain;

        public WGPUStringView tracePath;

        public WGPUNativeMemoryHint memoryHint;

        [NativeTypeName("uint64_t")]
        public ulong suballocatedBlockSizeMin;

        [NativeTypeName("uint64_t")]
        public ulong suballocatedBlockSizeMax;
    }
}
