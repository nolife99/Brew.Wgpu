#nullable disable
namespace Brew.Wgpu;

public ref struct BufferDescriptor
{
    public ulong Size;
    public BufferUsage Usage;
    public bool MappedAtCreation;
}
