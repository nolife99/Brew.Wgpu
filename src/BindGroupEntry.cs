#nullable disable
namespace Brew.Wgpu;

public struct BindGroupEntry
{
    public uint Binding;
    public BindGroupEntry.Kind Type;
    public Brew.Wgpu.Buffer BufferResource;
    public ulong BufferOffset;
    public ulong BufferSize;
    public Brew.Wgpu.Sampler SamplerResource;
    public Brew.Wgpu.TextureView TextureViewResource;

    public static BindGroupEntry Buffer(uint binding, Brew.Wgpu.Buffer buffer, ulong offset = 0, ulong size = 0)
    {
        return new BindGroupEntry()
        {
            Binding = binding,
            Type = BindGroupEntry.Kind.Buffer,
            BufferResource = buffer,
            BufferOffset = offset,
            BufferSize = size == 0UL ? ulong.MaxValue : size
        };
    }

    public static BindGroupEntry Sampler(uint binding, Brew.Wgpu.Sampler sampler)
    {
        return new BindGroupEntry()
        {
            Binding = binding,
            Type = BindGroupEntry.Kind.Sampler,
            SamplerResource = sampler
        };
    }

    public static BindGroupEntry TextureView(uint binding, Brew.Wgpu.TextureView view)
    {
        return new BindGroupEntry()
        {
            Binding = binding,
            Type = BindGroupEntry.Kind.TextureView,
            TextureViewResource = view
        };
    }

    public enum Kind : byte
    {
        Buffer,
        Sampler,
        TextureView,
    }
}
