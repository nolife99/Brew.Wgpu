using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum ColorWriteMask : ulong
{
    None = 0,
    Red = 1,
    Green = 2,
    Blue = 4,
    Alpha = 8,
    All = Alpha | Blue | Green | Red, // 0x000000000000000F
}
