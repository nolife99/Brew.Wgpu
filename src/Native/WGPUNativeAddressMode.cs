namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeAddressMode : uint
    {
        ClampToBorder = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
