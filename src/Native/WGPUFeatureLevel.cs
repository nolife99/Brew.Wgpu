namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUFeatureLevel : uint
    {
        Undefined = 0x00000000,
        Compatibility = 0x00000001,
        Core = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
