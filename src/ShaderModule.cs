using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>A compiled shader module. The handle is a value type ({slot, generation}); copies are free, disposal releases exactly once and invalidates all copies.</summary>
public readonly struct ShaderModule : IDisposable, IEquatable<ShaderModule>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe ShaderModule(WGPUShaderModuleImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUShaderModuleImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUShaderModuleImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUShaderModuleImpl* Handle
    {
        get
        {
            WGPUShaderModuleImpl* p = HandleTable<WGPUShaderModuleImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(ShaderModule));
            return p;
        }
    }

    public unsafe void Dispose()
    {
        WGPUShaderModuleImpl* p = HandleTable<WGPUShaderModuleImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuShaderModuleRelease(p);
    }

    public bool Equals(ShaderModule other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is ShaderModule o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(ShaderModule left, ShaderModule right) => left.Equals(right);
    public static bool operator !=(ShaderModule left, ShaderModule right) => !left.Equals(right);
}
