using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public readonly struct SurfaceAcquireResult
{
    public readonly Texture Texture;
    public readonly WGPUSurfaceGetCurrentTextureStatus Status;

    internal SurfaceAcquireResult(Texture texture, WGPUSurfaceGetCurrentTextureStatus status)
    {
        this.Texture = texture;
        this.Status = status;
    }

    public bool IsUsable => (int)this.Status - 1 <= 1;

    public bool NeedsReconfigure => (int)this.Status - 4 <= 1;

    public bool IsSuboptimal => (int)this.Status == 2;
}
