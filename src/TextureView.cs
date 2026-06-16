using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>A texture view. The handle is a value type ({slot, generation}); copies are free, disposal releases exactly once and invalidates all copies.</summary>
public readonly struct TextureView : IDisposable, IEquatable<TextureView>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe TextureView(WGPUTextureViewImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUTextureViewImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUTextureViewImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUTextureViewImpl* Handle
    {
        get
        {
            WGPUTextureViewImpl* p = HandleTable<WGPUTextureViewImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(TextureView));
            return p;
        }
    }

    public unsafe void Dispose()
    {
        WGPUTextureViewImpl* p = HandleTable<WGPUTextureViewImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuTextureViewRelease(p);
    }

    public bool Equals(TextureView other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is TextureView o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(TextureView left, TextureView right) => left.Equals(right);
    public static bool operator !=(TextureView left, TextureView right) => !left.Equals(right);
}
