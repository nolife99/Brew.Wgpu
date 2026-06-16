namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUCreateRenderPipelineAsyncCallbackInfo
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUCallbackMode mode;

        [NativeTypeName("WGPUCreateRenderPipelineAsyncCallback")]
        public delegate* unmanaged[Cdecl]<WGPUCreatePipelineAsyncStatus, WGPURenderPipelineImpl*, WGPUStringView, void*, void*, void> callback;

        public void* userdata1;

        public void* userdata2;
    }
}
