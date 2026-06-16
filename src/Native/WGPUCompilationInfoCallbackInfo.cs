namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUCompilationInfoCallbackInfo
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUCallbackMode mode;

        [NativeTypeName("WGPUCompilationInfoCallback")]
        public delegate* unmanaged[Cdecl]<WGPUCompilationInfoRequestStatus, WGPUCompilationInfo*, void*, void*, void> callback;

        public void* userdata1;

        public void* userdata2;
    }
}
