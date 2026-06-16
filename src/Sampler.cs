using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>A texture sampler. The handle is a value type ({slot, generation}); copies are free, disposal releases exactly once and invalidates all copies.</summary>
public readonly struct Sampler : IDisposable, IEquatable<Sampler>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe Sampler(WGPUSamplerImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUSamplerImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUSamplerImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUSamplerImpl* Handle
    {
        get
        {
            WGPUSamplerImpl* p = HandleTable<WGPUSamplerImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(Sampler));
            return p;
        }
    }

    public unsafe void Dispose()
    {
        WGPUSamplerImpl* p = HandleTable<WGPUSamplerImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuSamplerRelease(p);
    }

    public bool Equals(Sampler other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is Sampler o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(Sampler left, Sampler right) => left.Equals(right);
    public static bool operator !=(Sampler left, Sampler right) => !left.Equals(right);
}
