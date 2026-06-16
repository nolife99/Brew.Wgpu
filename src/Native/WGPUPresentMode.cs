namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUPresentMode : uint
    {
        Undefined = 0x00000000,
        Fifo = 0x00000001,
        FifoRelaxed = 0x00000002,
        Immediate = 0x00000003,
        Mailbox = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
