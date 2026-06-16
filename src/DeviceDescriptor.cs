using Brew.Wgpu.Native;
using System;

#nullable disable
namespace Brew.Wgpu;

public ref struct DeviceDescriptor
{
    public unsafe delegate* unmanaged[Cdecl]<WGPUDeviceImpl**, WGPUErrorType, WGPUStringView, void*, void*, void> UncapturedErrorCallback;
    public unsafe void* UncapturedErrorUserdata;
    public ReadOnlySpan<WGPUFeatureName> RequiredFeatures;
    public WGPULimits? RequiredLimits;
    public WGPUNativeLimits? RequiredNativeLimits;
    public WGPUNativeMemoryHint MemoryHint;
}
