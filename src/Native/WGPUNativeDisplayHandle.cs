using System.Runtime.InteropServices;

namespace Brew.Wgpu.Native
{
    public partial struct WGPUNativeDisplayHandle
    {
        public WGPUNativeDisplayHandleType type;

        [NativeTypeName("__AnonymousRecord_wgpu_L1116_C5")]
        public _data_e__Union data;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _data_e__Union
        {
            [FieldOffset(0)]
            public WGPUXlibDisplayHandle xlib;

            [FieldOffset(0)]
            public WGPUXcbDisplayHandle xcb;

            [FieldOffset(0)]
            public WGPUWaylandDisplayHandle wayland;
        }
    }
}
