namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUGLFenceBehaviour : uint
    {
        Normal = 0x00000000,
        AutoFinish = 0x00000001,
        Force32 = 0x7FFFFFFF,
    }
}
