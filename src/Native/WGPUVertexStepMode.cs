namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUVertexStepMode : uint
    {
        Undefined = 0x00000000,
        Vertex = 0x00000001,
        Instance = 0x00000002,
        Force32 = 0x7FFFFFFF,
    }
}
