namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUErrorFilter : uint
    {
        Validation = 0x00000001,
        OutOfMemory = 0x00000002,
        Internal = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
