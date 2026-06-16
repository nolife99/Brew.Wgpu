namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCompareFunction : uint
    {
        Undefined = 0x00000000,
        Never = 0x00000001,
        Less = 0x00000002,
        Equal = 0x00000003,
        LessEqual = 0x00000004,
        Greater = 0x00000005,
        NotEqual = 0x00000006,
        GreaterEqual = 0x00000007,
        Always = 0x00000008,
        Force32 = 0x7FFFFFFF,
    }
}
