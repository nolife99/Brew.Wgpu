namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUErrorType : uint
    {
        NoError = 0x00000001,
        Validation = 0x00000002,
        OutOfMemory = 0x00000003,
        Internal = 0x00000004,
        Unknown = 0x00000005,
        Force32 = 0x7FFFFFFF,
    }
}
