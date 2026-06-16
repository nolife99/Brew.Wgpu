namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUBlendOperation : uint
    {
        Undefined = 0x00000000,
        Add = 0x00000001,
        Subtract = 0x00000002,
        ReverseSubtract = 0x00000003,
        Min = 0x00000004,
        Max = 0x00000005,
        Force32 = 0x7FFFFFFF,
    }
}
