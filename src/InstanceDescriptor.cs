using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct InstanceDescriptor
{
    public InstanceFlags Flags;
    public InstanceBackends Backends;
    public unsafe delegate* unmanaged[Cdecl]<WGPULogLevel, WGPUStringView, void*, void> LogCallback;
    public WGPULogLevel LogLevel;
    public unsafe void* LogUserdata;
}
