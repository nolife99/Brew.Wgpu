namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUOptionalBool : uint
    {
        WGPUOptionalBool_False = 0x00000000,
        WGPUOptionalBool_True = 0x00000001,
        WGPUOptionalBool_Undefined = 0x00000002,
        WGPUOptionalBool_Force32 = 0x7FFFFFFF,
    }
}
