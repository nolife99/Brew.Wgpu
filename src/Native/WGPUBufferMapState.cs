namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUBufferMapState : uint
    {
        Unmapped = 0x00000001,
        Pending = 0x00000002,
        Mapped = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
