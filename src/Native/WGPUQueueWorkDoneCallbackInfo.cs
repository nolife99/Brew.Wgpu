namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUQueueWorkDoneCallbackInfo
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUCallbackMode mode;

        [NativeTypeName("WGPUQueueWorkDoneCallback")]
        public delegate* unmanaged[Cdecl]<WGPUQueueWorkDoneStatus, WGPUStringView, void*, void*, void> callback;

        public void* userdata1;

        public void* userdata2;
    }
}
