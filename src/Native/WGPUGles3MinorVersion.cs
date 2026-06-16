namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUGles3MinorVersion : uint
    {
        Automatic = 0x00000000,
        Version0 = 0x00000001,
        Version1 = 0x00000002,
        Version2 = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
