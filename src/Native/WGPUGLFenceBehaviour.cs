namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUGLFenceBehaviour : uint
    {
        WGPUGLFenceBehaviour_Normal = 0x00000000,
        WGPUGLFenceBehaviour_AutoFinish = 0x00000001,
        WGPUGLFenceBehaviour_Force32 = 0x7FFFFFFF,
    }
}
