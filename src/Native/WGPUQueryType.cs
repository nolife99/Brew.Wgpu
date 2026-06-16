namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUQueryType : uint
    {
        WGPUQueryType_Occlusion = 0x00000001,
        WGPUQueryType_Timestamp = 0x00000002,
        WGPUQueryType_Force32 = 0x7FFFFFFF,
    }
}
