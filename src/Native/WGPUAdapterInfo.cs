namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUAdapterInfo
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView vendor;

        public WGPUStringView architecture;

        public WGPUStringView device;

        public WGPUStringView description;

        public WGPUBackendType backendType;

        public WGPUAdapterType adapterType;

        [NativeTypeName("uint32_t")]
        public uint vendorID;

        [NativeTypeName("uint32_t")]
        public uint deviceID;

        [NativeTypeName("uint32_t")]
        public uint subgroupMinSize;

        [NativeTypeName("uint32_t")]
        public uint subgroupMaxSize;
    }
}
