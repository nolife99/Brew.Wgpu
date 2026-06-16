using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum MapMode : ulong
{
    None = 0,
    Read = 1,
    Write = 2,
}
