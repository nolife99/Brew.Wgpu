namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUDx12SwapchainKind : uint
    {
        WGPUDx12SwapchainKind_Undefined = 0x00000000,
        WGPUDx12SwapchainKind_DxgiFromHwnd = 0x00000001,
        WGPUDx12SwapchainKind_DxgiFromVisual = 0x00000002,
        WGPUDx12SwapchainKind_Force32 = 0x7FFFFFFF,
    }
}
