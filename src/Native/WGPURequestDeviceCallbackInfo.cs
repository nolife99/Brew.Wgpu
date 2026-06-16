namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPURequestDeviceCallbackInfo
    {
        public WGPUChainedStruct* nextInChain;

        public WGPUCallbackMode mode;

        [NativeTypeName("WGPURequestDeviceCallback")]
        public delegate* unmanaged[Cdecl]<WGPURequestDeviceStatus, WGPUDeviceImpl*, WGPUStringView, void*, void*, void> callback;

        public void* userdata1;

        public void* userdata2;
    }
}
