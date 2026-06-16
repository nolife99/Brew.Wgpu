namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFrontFace : uint
    {
        Undefined = 0x00000000,
        CCW = 0x00000001,
        CW = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
