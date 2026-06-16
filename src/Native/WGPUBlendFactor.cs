namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUBlendFactor : uint
    {
        Undefined = 0x00000000,
        Zero = 0x00000001,
        One = 0x00000002,
        Src = 0x00000003,
        OneMinusSrc = 0x00000004,
        SrcAlpha = 0x00000005,
        OneMinusSrcAlpha = 0x00000006,
        Dst = 0x00000007,
        OneMinusDst = 0x00000008,
        DstAlpha = 0x00000009,
        OneMinusDstAlpha = 0x0000000A,
        SrcAlphaSaturated = 0x0000000B,
        Constant = 0x0000000C,
        OneMinusConstant = 0x0000000D,
        Src1 = 0x0000000E,
        OneMinusSrc1 = 0x0000000F,
        Src1Alpha = 0x00000010,
        OneMinusSrc1Alpha = 0x00000011,
        Force32 = 0x7FFFFFFF,
    }
}
