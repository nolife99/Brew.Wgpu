namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUCullMode : uint
    {
        Undefined = 0x00000000,
        None = 0x00000001,
        Front = 0x00000002,
        Back = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
