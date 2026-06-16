namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUDx12SwapchainKind : uint
    {
        Undefined = 0x00000000,
        DxgiFromHwnd = 0x00000001,
        DxgiFromVisual = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
