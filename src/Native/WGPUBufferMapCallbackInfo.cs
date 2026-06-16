namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUBufferMapCallbackInfo
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUCallbackMode mode;

        [NativeTypeName("WGPUBufferMapCallback")]
        public delegate* unmanaged[Cdecl]<WGPUMapAsyncStatus, WGPUStringView, void*, void*, void> callback;

        public void* userdata1;

        public void* userdata2;
    }
}
