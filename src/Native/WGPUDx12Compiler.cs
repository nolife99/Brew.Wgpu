namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUDx12Compiler : uint
    {
        Undefined = 0x00000000,
        Fxc = 0x00000001,
        Dxc = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
