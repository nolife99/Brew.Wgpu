namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUPassTimestampWrites
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("WGPUQuerySet")]
        public WGPUQuerySetImpl* querySet;

        [NativeTypeName("uint32_t")]
        public uint beginningOfPassWriteIndex;

        [NativeTypeName("uint32_t")]
        public uint endOfPassWriteIndex;
    }
}
