namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFrontFace : uint
    {
        WGPUFrontFace_Undefined = 0x00000000,
        WGPUFrontFace_CCW = 0x00000001,
        WGPUFrontFace_CW = 0x00000002,
        WGPUFrontFace_Force32 = 0x7FFFFFFF,
    }
}
