using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum ShaderStage : ulong
{
    None = 0,
    Vertex = 1,
    Fragment = 2,
    Compute = 4,
}
