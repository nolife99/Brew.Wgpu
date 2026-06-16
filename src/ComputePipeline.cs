using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>A compute pipeline. The handle is a value type ({slot, generation}); copies are free, disposal releases exactly once and invalidates all copies.</summary>
public readonly struct ComputePipeline : IDisposable, IEquatable<ComputePipeline>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe ComputePipeline(WGPUComputePipelineImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPUComputePipelineImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPUComputePipelineImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPUComputePipelineImpl* Handle
    {
        get
        {
            WGPUComputePipelineImpl* p = HandleTable<WGPUComputePipelineImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(ComputePipeline));
            return p;
        }
    }

    public unsafe void Dispose()
    {
        WGPUComputePipelineImpl* p = HandleTable<WGPUComputePipelineImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuComputePipelineRelease(p);
    }

    public bool Equals(ComputePipeline other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is ComputePipeline o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(ComputePipeline left, ComputePipeline right) => left.Equals(right);
    public static bool operator !=(ComputePipeline left, ComputePipeline right) => !left.Equals(right);
}
