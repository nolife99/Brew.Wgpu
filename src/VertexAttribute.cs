using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct VertexAttribute(WGPUVertexFormat format, ulong offset, uint shaderLocation)
{
    public WGPUVertexFormat Format = format;
    public ulong Offset = offset;
    public uint ShaderLocation = shaderLocation;
}
