namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPULoadOp : uint
    {
        WGPULoadOp_Undefined = 0x00000000,
        WGPULoadOp_Load = 0x00000001,
        WGPULoadOp_Clear = 0x00000002,
        WGPULoadOp_Force32 = 0x7FFFFFFF,
    }
}
