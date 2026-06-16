using Brew.Wgpu.Handles;
using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

/// <summary>A render pipeline. The handle is a value type ({slot, generation}); copies are free, disposal releases exactly once and invalidates all copies.</summary>
public readonly struct RenderPipeline : IDisposable, IEquatable<RenderPipeline>
{
    private readonly int _slot;
    private readonly uint _gen;

    internal unsafe RenderPipeline(WGPURenderPipelineImpl* handle)
    {
        if (handle is null) { _slot = 0; _gen = 0; }
        else { (_slot, _gen) = HandleTable<WGPURenderPipelineImpl>.Register(handle); }
    }

    public unsafe bool IsNull => _gen == 0 || HandleTable<WGPURenderPipelineImpl>.Resolve(_slot, _gen) is null;

    /// <summary>Live native pointer; throws <see cref="ObjectDisposedException"/> after disposal.</summary>
    public unsafe WGPURenderPipelineImpl* Handle
    {
        get
        {
            WGPURenderPipelineImpl* p = HandleTable<WGPURenderPipelineImpl>.Resolve(_slot, _gen);
            if (p is null) throw new ObjectDisposedException(nameof(RenderPipeline));
            return p;
        }
    }

    public unsafe void Dispose()
    {
        WGPURenderPipelineImpl* p = HandleTable<WGPURenderPipelineImpl>.Retire(_slot, _gen);
        if (p is not null) WGPU.wgpuRenderPipelineRelease(p);
    }

    public bool Equals(RenderPipeline other) => _slot == other._slot && _gen == other._gen;
    public override bool Equals(object? obj) => obj is RenderPipeline o && Equals(o);
    public override int GetHashCode() => HashCode.Combine(_slot, _gen);
    public static bool operator ==(RenderPipeline left, RenderPipeline right) => left.Equals(right);
    public static bool operator !=(RenderPipeline left, RenderPipeline right) => !left.Equals(right);
}
