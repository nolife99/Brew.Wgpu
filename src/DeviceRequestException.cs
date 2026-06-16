using Brew.Wgpu.Native;
using System;

namespace Brew.Wgpu;

public sealed class DeviceRequestException : Exception
{
    public DeviceRequestException(WGPURequestDeviceStatus status, string? message = null)
        : base(message ?? $"Device request failed (status={status}).")
    {
        Status = status;
    }

    public WGPURequestDeviceStatus Status { get; }
}
