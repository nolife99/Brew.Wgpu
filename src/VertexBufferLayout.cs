using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct VertexBufferLayout(
    ulong arrayStride,
    int attributeCount,
    WGPUVertexStepMode stepMode = WGPUVertexStepMode.Vertex)
{
    public ulong ArrayStride = arrayStride;
    public WGPUVertexStepMode StepMode = stepMode;
    public int AttributeCount = attributeCount;
}
