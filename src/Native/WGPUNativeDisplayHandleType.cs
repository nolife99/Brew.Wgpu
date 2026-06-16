namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeDisplayHandleType : uint
    {
        WGPUNativeDisplayHandleType_None = 0x00000000,
        WGPUNativeDisplayHandleType_Xlib = 0x00000001,
        WGPUNativeDisplayHandleType_Xcb = 0x00000002,
        WGPUNativeDisplayHandleType_Wayland = 0x00000003,
        WGPUNativeDisplayHandleType_Force32 = 0x7FFFFFFF,
    }
}
