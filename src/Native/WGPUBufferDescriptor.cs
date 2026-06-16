namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUBufferDescriptor
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUStringView label;

        [NativeTypeName("WGPUBufferUsage")]
        public ulong usage;

        [NativeTypeName("uint64_t")]
        public ulong size;

        [NativeTypeName("WGPUBool")]
        public uint mappedAtCreation;
    }
}
