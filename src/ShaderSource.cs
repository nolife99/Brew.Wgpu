using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Brew.Wgpu;

public readonly ref struct ShaderSource
{
    public readonly ShaderSource.Kind Tag;
    public readonly ReadOnlySpan<byte> Bytes;

    private ShaderSource(ShaderSource.Kind tag, ReadOnlySpan<byte> bytes)
    {
        this.Tag = tag;
        this.Bytes = bytes;
    }

    public static ShaderSource FromWgsl(ReadOnlySpan<byte> utf8Code)
    {
        return new ShaderSource(ShaderSource.Kind.Wgsl, utf8Code);
    }

    public static ShaderSource FromSpirV(ReadOnlySpan<byte> bytes)
    {
        return new ShaderSource(ShaderSource.Kind.SpirV, bytes);
    }

    public static ShaderSource FromSpirV(ReadOnlySpan<uint> words)
    {
        return new ShaderSource(ShaderSource.Kind.SpirV, MemoryMarshal.AsBytes<uint>(words));
    }

    public static ShaderSource FromSpirVPassthrough(ReadOnlySpan<byte> bytes)
    {
        return new ShaderSource(ShaderSource.Kind.SpirVPassthrough, bytes);
    }

    public static ShaderSource FromSpirVPassthrough(ReadOnlySpan<uint> words)
    {
        return new ShaderSource(ShaderSource.Kind.SpirVPassthrough, MemoryMarshal.AsBytes<uint>(words));
    }

    public enum Kind : byte
    {
        Wgsl,
        SpirV,
        SpirVPassthrough,
    }
}
