using System;

#nullable enable
namespace Brew.Wgpu;

public sealed class ResourceCreationException(
    string resourceType,
    string nativeFunction,
    string? message = null) : Exception(message ?? $"{resourceType} creation failed: {nativeFunction} returned null. Check the device's uncaptured-error callback for details.")
{
    public string ResourceType { get; } = resourceType;

    public string NativeFunction { get; } = nativeFunction;
}
