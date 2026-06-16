namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUCreateComputePipelineAsyncCallbackInfo
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUCallbackMode mode;

        [NativeTypeName("WGPUCreateComputePipelineAsyncCallback")]
        public delegate* unmanaged[Cdecl]<WGPUCreatePipelineAsyncStatus, WGPUComputePipelineImpl*, WGPUStringView, void*, void*, void> callback;

        public void* userdata1;

        public void* userdata2;
    }
}
