namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUIndexFormat : uint
    {
        WGPUIndexFormat_Undefined = 0x00000000,
        WGPUIndexFormat_Uint16 = 0x00000001,
        WGPUIndexFormat_Uint32 = 0x00000002,
        WGPUIndexFormat_Force32 = 0x7FFFFFFF,
    }
}
