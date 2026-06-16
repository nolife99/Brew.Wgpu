namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUDx12Compiler : uint
    {
        WGPUDx12Compiler_Undefined = 0x00000000,
        WGPUDx12Compiler_Fxc = 0x00000001,
        WGPUDx12Compiler_Dxc = 0x00000002,
        WGPUDx12Compiler_Force32 = 0x7FFFFFFF,
    }
}
