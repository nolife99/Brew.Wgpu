namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUGles3MinorVersion : uint
    {
        WGPUGles3MinorVersion_Automatic = 0x00000000,
        WGPUGles3MinorVersion_Version0 = 0x00000001,
        WGPUGles3MinorVersion_Version1 = 0x00000002,
        WGPUGles3MinorVersion_Version2 = 0x00000003,
        WGPUGles3MinorVersion_Force32 = 0x7FFFFFFF,
    }
}
