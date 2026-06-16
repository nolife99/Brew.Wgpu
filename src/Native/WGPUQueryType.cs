namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUQueryType : uint
    {
        Occlusion = 0x00000001,
        Timestamp = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
