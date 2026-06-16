using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum FormatAspect : byte
{
    None = 0,
    Color = 1,
    Depth = 2,
    Stencil = 4,
}
