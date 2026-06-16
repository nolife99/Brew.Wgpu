namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUDxcMaxShaderModel : uint
    {
        V6_0 = 0x00000000,
        V6_1 = 0x00000001,
        V6_2 = 0x00000002,
        V6_3 = 0x00000003,
        V6_4 = 0x00000004,
        V6_5 = 0x00000005,
        V6_6 = 0x00000006,
        V6_7 = 0x00000007,
        Force32 = 0x7FFFFFFF,
    }
}
