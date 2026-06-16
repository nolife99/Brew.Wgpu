namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeAddressMode : uint
    {
        WGPUNativeAddressMode_ClampToBorder = 0x00000004,
        WGPUNativeAddressMode_Force32 = 0x7FFFFFFF,
    }
}
