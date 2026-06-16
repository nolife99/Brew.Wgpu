namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUTextureDimension : uint
    {
        Undefined = 0x00000000,
        _1D = 0x00000001,
        _2D = 0x00000002,
        _3D = 0x00000003,
        Force32 = 0x7FFFFFFF,
    }
}
