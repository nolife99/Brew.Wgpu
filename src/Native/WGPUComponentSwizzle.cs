namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUComponentSwizzle : uint
    {
        WGPUComponentSwizzle_Undefined = 0x00000000,
        WGPUComponentSwizzle_Zero = 0x00000001,
        WGPUComponentSwizzle_One = 0x00000002,
        WGPUComponentSwizzle_R = 0x00000003,
        WGPUComponentSwizzle_G = 0x00000004,
        WGPUComponentSwizzle_B = 0x00000005,
        WGPUComponentSwizzle_A = 0x00000006,
        WGPUComponentSwizzle_Force32 = 0x7FFFFFFF,
    }
}
