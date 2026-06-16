using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum TextureUsage : ulong
{
    None = 0,
    CopySrc = 1,
    CopyDst = 2,
    TextureBinding = 4,
    StorageBinding = 8,
    RenderAttachment = 16, // 0x0000000000000010
    TransientAttachment = 32, // 0x0000000000000020
    StorageAtomic = 64, // 0x0000000000000040
}
