namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUComponentSwizzle : uint
    {
        Undefined = 0x00000000,
        Zero = 0x00000001,
        One = 0x00000002,
        R = 0x00000003,
        G = 0x00000004,
        B = 0x00000005,
        A = 0x00000006,
        Force32 = 0x7FFFFFFF,
    }
}
