using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum BufferUsage : ulong
{
    None = 0,
    MapRead = 1,
    MapWrite = 2,
    CopySrc = 4,
    CopyDst = 8,
    Index = 16, // 0x0000000000000010
    Vertex = 32, // 0x0000000000000020
    Uniform = 64, // 0x0000000000000040
    Storage = 128, // 0x0000000000000080
    Indirect = 256, // 0x0000000000000100
    QueryResolve = 512, // 0x0000000000000200
}
