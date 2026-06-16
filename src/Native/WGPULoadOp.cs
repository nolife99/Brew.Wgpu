namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPULoadOp : uint
    {
        Undefined = 0x00000000,
        Load = 0x00000001,
        Clear = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
