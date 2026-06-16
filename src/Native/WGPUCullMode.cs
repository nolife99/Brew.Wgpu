namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCullMode : uint
    {
        WGPUCullMode_Undefined = 0x00000000,
        WGPUCullMode_None = 0x00000001,
        WGPUCullMode_Front = 0x00000002,
        WGPUCullMode_Back = 0x00000003,
        WGPUCullMode_Force32 = 0x7FFFFFFF,
    }
}
