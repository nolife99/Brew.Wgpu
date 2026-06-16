namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUInstanceExtras
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("WGPUInstanceBackend")]
        public ulong backends;

        [NativeTypeName("WGPUInstanceFlag")]
        public ulong flags;

        public WGPUDx12Compiler dx12ShaderCompiler;

        public WGPUGles3MinorVersion gles3MinorVersion;

        public WGPUGLFenceBehaviour glFenceBehaviour;

        public WGPUStringView dxcPath;

        public WGPUDxcMaxShaderModel dxcMaxShaderModel;

        public WGPUDx12SwapchainKind dx12PresentationSystem;

        [NativeTypeName("const uint8_t *")]
        public byte* budgetForDeviceCreation;

        [NativeTypeName("const uint8_t *")]
        public byte* budgetForDeviceLoss;

        public WGPUNativeDisplayHandle displayHandle;
    }
}
