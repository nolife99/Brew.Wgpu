using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum InstanceBackends : ulong
{
    None = 0,
    Vulkan = 1,
    GL = 2,
    Metal = 4,
    Dx12 = 8,
    BrowserWebGpu = 32, // 0x0000000000000020
    Primary = BrowserWebGpu | Dx12 | Metal | Vulkan, // 0x000000000000002D
    All = Primary | GL, // 0x000000000000002F
}
