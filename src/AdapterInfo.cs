using Brew.Wgpu.Native;

namespace Brew.Wgpu;

/// <summary>Information about a physical adapter.</summary>
public readonly record struct AdapterInfo(
    string Vendor,
    string Architecture,
    string Device,
    string Description,
    WGPUBackendType Backend,
    WGPUAdapterType Type,
    uint VendorID,
    uint DeviceID,
    uint SubgroupMinSize,
    uint SubgroupMaxSize);
