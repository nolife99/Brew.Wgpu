namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUBufferBindingType : uint
    {
        BindingNotUsed = 0x00000000,
        Undefined = 0x00000001,
        Uniform = 0x00000002,
        Storage = 0x00000003,
        ReadOnlyStorage = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
