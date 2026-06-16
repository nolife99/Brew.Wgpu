namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUStoreOp : uint
    {
        Undefined = 0x00000000,
        Store = 0x00000001,
        Discard = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
