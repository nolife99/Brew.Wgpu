namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUBufferMapState : uint
    {
        WGPUBufferMapState_Unmapped = 0x00000001,
        WGPUBufferMapState_Pending = 0x00000002,
        WGPUBufferMapState_Mapped = 0x00000003,
        WGPUBufferMapState_Force32 = 0x7FFFFFFF,
    }
}
