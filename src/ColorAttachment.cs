using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct ColorAttachment(
    TextureView view,
    WGPULoadOp loadOp,
    WGPUStoreOp storeOp,
    WGPUColor clearValue)
{
    public TextureView View = view;
    public WGPULoadOp LoadOp = loadOp;
    public WGPUStoreOp StoreOp = storeOp;
    public WGPUColor ClearValue = clearValue;
    public TextureView ResolveTarget = new TextureView();
}
