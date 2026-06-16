using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>A bind group. The handle is a value type ({slot, generation}); copies are free, disposal releases exactly once and invalidates all copies.</summary>
public readonly struct BindGroup : IDisposable, IEquatable<BindGroup>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe BindGroup(WGPUBindGroupImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUBindGroupImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUBindGroupImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUBindGroupImpl* Handle
    {
        get
        {
            WGPUBindGroupImpl* p = HandleTable<WGPUBindGroupImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(BindGroup));
            return p;
        }
    }

    public unsafe void Dispose()
    {
        WGPUBindGroupImpl* p = HandleTable<WGPUBindGroupImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuBindGroupRelease(p);
    }

    public bool Equals(BindGroup other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is BindGroup o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(BindGroup left, BindGroup right) => left.Equals(right);
    public static bool operator !=(BindGroup left, BindGroup right) => !left.Equals(right);
}
