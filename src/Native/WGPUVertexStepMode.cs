namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUVertexStepMode : uint
    {
        WGPUVertexStepMode_Undefined = 0x00000000,
        WGPUVertexStepMode_Vertex = 0x00000001,
        WGPUVertexStepMode_Instance = 0x00000002,
        WGPUVertexStepMode_Force32 = 0x7FFFFFFF,
    }
}
