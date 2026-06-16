using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct VertexBufferLayout(
    ulong arrayStride,
    int attributeCount,
    WGPUVertexStepMode stepMode = (WGPUVertexStepMode)1)
{
    public ulong ArrayStride = arrayStride;
    public WGPUVertexStepMode StepMode = stepMode;
    public int AttributeCount = attributeCount;
}
