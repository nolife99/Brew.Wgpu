namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUShaderSourceGLSL
    {
        public WGPUChainedStruct chain;

        [NativeTypeName("WGPUShaderStage")]
        public ulong stage;

        public WGPUStringView code;

        [NativeTypeName("uint32_t")]
        public uint defineCount;

        [NativeTypeName("const WGPUShaderDefine *")]
        public WGPUShaderDefine* defines;
    }
}
