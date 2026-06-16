namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUOptionalBool : uint
    {
        False = 0x00000000,
        True = 0x00000001,
        Undefined = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
