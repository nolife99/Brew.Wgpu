namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUUncapturedErrorCallbackInfo
    {
        public WGPUChainedStruct* nextInChain;

        [NativeTypeName("WGPUUncapturedErrorCallback")]
        public delegate* unmanaged[Cdecl]<WGPUDeviceImpl**, WGPUErrorType, WGPUStringView, void*, void*, void> callback;

        public void* userdata1;

        public void* userdata2;
    }
}
