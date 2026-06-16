using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct DepthStencilAttachment
{
    public TextureView View;
    public WGPULoadOp DepthLoadOp;
    public WGPUStoreOp DepthStoreOp;
    public float DepthClearValue;
    public bool DepthReadOnly;
    public WGPULoadOp StencilLoadOp;
    public WGPUStoreOp StencilStoreOp;
    public uint StencilClearValue;
    public bool StencilReadOnly;

    public static DepthStencilAttachment DepthClear(
      TextureView view,
      float clearValue = 1f,
      WGPUStoreOp storeOp = WGPUStoreOp.Store)
    {
        return new DepthStencilAttachment()
        {
            View = view,
            DepthLoadOp = WGPULoadOp.Clear,
            DepthStoreOp = storeOp,
            DepthClearValue = clearValue
        };
    }

    public static DepthStencilAttachment DepthReadOnlyOf(TextureView view)
    {
        return new DepthStencilAttachment()
        {
            View = view,
            DepthReadOnly = true
        };
    }
}
