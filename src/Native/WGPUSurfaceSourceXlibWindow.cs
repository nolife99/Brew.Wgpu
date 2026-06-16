namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUSurfaceSourceXlibWindow
    {
        public WGPUChainedStruct chain;

        public void* display;

        [NativeTypeName("uint64_t")]
        public ulong window;
    }
}
