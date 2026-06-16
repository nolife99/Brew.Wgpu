using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

public sealed class AdapterRequestException : Exception
{
    public AdapterRequestException(WGPURequestAdapterStatus status, string? message = null)
        : base(message ?? $"Adapter request failed (status={status}).")
    {
        Status = status;
    }

    public WGPURequestAdapterStatus Status { get; }
}
