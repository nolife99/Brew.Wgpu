namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUIndexFormat : uint
    {
        Undefined = 0x00000000,
        Uint16 = 0x00000001,
        Uint32 = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
