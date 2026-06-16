namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUNativeDisplayHandleType : uint
    {
        None = 0x00000000,
        Xlib = 0x00000001,
        Xcb = 0x00000002,
        Wayland = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
