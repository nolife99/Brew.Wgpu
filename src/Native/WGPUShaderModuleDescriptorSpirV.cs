namespace Brew.Wgpu.Native
{
    public unsafe partial struct WGPUShaderModuleDescriptorSpirV
    {
        public WGPUStringView label;

        [NativeTypeName("uint32_t")]
        public uint sourceSize;

        [NativeTypeName("const uint32_t *")]
        public uint* source;
    }
}
