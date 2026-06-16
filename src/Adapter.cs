using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

#nullable enable
namespace Brew.Wgpu;

public sealed class Adapter : IDisposable
{
    private unsafe WGPUAdapterImpl* _handle;
    private readonly Instance _instance;

    public unsafe WGPUAdapterImpl* Handle => this._handle;

    internal unsafe Adapter(WGPUAdapterImpl* handle, Instance instance)
    {
        this._handle = handle;
        this._instance = instance;
    }

    public Device RequestDeviceBlocking() => this.RequestDeviceBlocking(new DeviceDescriptor());

    public unsafe Device RequestDeviceBlocking(in DeviceDescriptor descriptor)
    {
        this.ThrowIfDisposed();
        Adapter.DeviceCallbackResult deviceCallbackResult = new Adapter.DeviceCallbackResult()
        {
            Status = (WGPURequestDeviceStatus)int.MaxValue
        };
        WGPURequestDeviceCallbackInfo deviceCallbackInfo = new WGPURequestDeviceCallbackInfo()
        {
            mode = (WGPUCallbackMode)2,
            callback = &OnDevice,
            userdata1 = Unsafe.AsPointer<Adapter.DeviceCallbackResult>(ref deviceCallbackResult)
        };
        bool flag = descriptor.RequiredLimits.HasValue || descriptor.RequiredNativeLimits.HasValue;
        if (((descriptor.UncapturedErrorCallback != null ? 1 : (descriptor.RequiredFeatures.Length > 0 ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        {
            WGPUStringView wgpuStringView = new WGPUStringView()
            {
                data = (sbyte*)null,
                length = WgpuDefaults.StrLen
            };
            fixed (WGPUFeatureName* wgpuFeatureNamePtr = &descriptor.RequiredFeatures.GetPinnableReference())
            {
                WGPULimits valueOrDefault1 = descriptor.RequiredLimits.GetValueOrDefault();
                WGPUNativeLimits valueOrDefault2 = descriptor.RequiredNativeLimits.GetValueOrDefault();
                if (descriptor.RequiredNativeLimits.HasValue)
                {
                    ref WGPUNativeLimits local = ref valueOrDefault2;
                    WGPUChainedStruct wgpuChainedStruct = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)196610 /*0x030002*/
                    };
                    local.chain = wgpuChainedStruct;
                    valueOrDefault1.nextInChain = (WGPUChainedStruct*)&valueOrDefault2;
                }
                WGPUDeviceDescriptor deviceDescriptor = new WGPUDeviceDescriptor();
                deviceDescriptor.label = wgpuStringView;
                ref WGPUDeviceDescriptor local1 = ref deviceDescriptor;
                WGPUQueueDescriptor wgpuQueueDescriptor = new WGPUQueueDescriptor()
                {
                    label = wgpuStringView
                };
                local1.defaultQueue = wgpuQueueDescriptor;
                deviceDescriptor.requiredFeatureCount = (UIntPtr)descriptor.RequiredFeatures.Length;
                deviceDescriptor.requiredFeatures = wgpuFeatureNamePtr;
                deviceDescriptor.requiredLimits = flag ? &valueOrDefault1 : (WGPULimits*)null;
                ref WGPUDeviceDescriptor local2 = ref deviceDescriptor;
                WGPUUncapturedErrorCallbackInfo errorCallbackInfo = new WGPUUncapturedErrorCallbackInfo()
                {
                    callback = descriptor.UncapturedErrorCallback,
                    userdata1 = descriptor.UncapturedErrorUserdata
                };
                local2.uncapturedErrorCallbackInfo = errorCallbackInfo;
                WGPU.wgpuAdapterRequestDevice(this.Handle, &deviceDescriptor, deviceCallbackInfo);
            }
        }
        else
            WGPU.wgpuAdapterRequestDevice(this.Handle, (WGPUDeviceDescriptor*)null, deviceCallbackInfo);
        Async.PollUntilChanged<WGPURequestDeviceStatus>(this._instance.Handle, ref deviceCallbackResult.Status, (WGPURequestDeviceStatus)int.MaxValue);
        if ((int)deviceCallbackResult.Status != 1 || (IntPtr)deviceCallbackResult.Device == IntPtr.Zero)
            throw new DeviceRequestException(deviceCallbackResult.Status);
        return new Device(deviceCallbackResult.Device, this._instance);
    }

    public unsafe bool HasFeature(WGPUFeatureName feature)
    {
        this.ThrowIfDisposed();
        return WGPU.wgpuAdapterHasFeature(this._handle, feature) > 0U;
    }

    public unsafe WGPUFeatureName[] GetSupportedFeatures()
    {
        this.ThrowIfDisposed();
        WGPUSupportedFeatures supportedFeatures1 = new WGPUSupportedFeatures();
        WGPU.wgpuAdapterGetFeatures(this._handle, &supportedFeatures1);
        WGPUFeatureName[] supportedFeatures2 = new WGPUFeatureName[(int)supportedFeatures1.featureCount];
        for (int index = 0; index < supportedFeatures2.Length; ++index)
            supportedFeatures2[index] = (WGPUFeatureName)((int*)supportedFeatures1.features)[index];
        WGPU.wgpuSupportedFeaturesFreeMembers(supportedFeatures1);
        return supportedFeatures2;
    }

    public unsafe WGPULimits GetLimits()
    {
        this.ThrowIfDisposed();
        WGPULimits limits = new WGPULimits();
        WGPU.wgpuAdapterGetLimits(this._handle, &limits);
        return limits;
    }

    public unsafe AdapterInfo GetInfo()
    {
        this.ThrowIfDisposed();
        WGPUAdapterInfo wgpuAdapterInfo = new WGPUAdapterInfo();
        WGPU.wgpuAdapterGetInfo(this._handle, &wgpuAdapterInfo);
        AdapterInfo info = new AdapterInfo(Adapter.DecodeString(wgpuAdapterInfo.vendor), Adapter.DecodeString(wgpuAdapterInfo.architecture), Adapter.DecodeString(wgpuAdapterInfo.device), Adapter.DecodeString(wgpuAdapterInfo.description), wgpuAdapterInfo.backendType, wgpuAdapterInfo.adapterType, wgpuAdapterInfo.vendorID, wgpuAdapterInfo.deviceID, wgpuAdapterInfo.subgroupMinSize, wgpuAdapterInfo.subgroupMaxSize);
        WGPU.wgpuAdapterInfoFreeMembers(wgpuAdapterInfo);
        return info;
    }

    public string Dump(bool includeLimits = true)
    {
        this.ThrowIfDisposed();
        AdapterInfo info = this.GetInfo();
        WGPULimits wgpuLimits = includeLimits ? this.GetLimits() : new WGPULimits();
        StringBuilder stringBuilder = new StringBuilder(includeLimits ? 1024 /*0x0400*/ : 256 /*0x0100*/);
        stringBuilder.Append("Adapter:       ").Append(info.Device).Append('\n');
        if (info.Description.Length > 0)
            stringBuilder.Append("Description:   ").Append(info.Description).Append('\n');
        if (info.Architecture.Length > 0)
            stringBuilder.Append("Architecture:  ").Append(info.Architecture).Append('\n');
        if (info.Vendor.Length > 0)
            stringBuilder.Append("Vendor:        ").Append(info.Vendor).Append('\n');
        stringBuilder.Append("Backend:       ").Append((object)info.Backend).Append('\n');
        stringBuilder.Append("Type:          ").Append((object)info.Type).Append('\n');
        stringBuilder.Append("Vendor/Device: 0x").AppendFormat("{0:X4}", (object)info.VendorID).Append(" / 0x").AppendFormat("{0:X4}", (object)info.DeviceID).Append('\n');
        if (info.SubgroupMaxSize > 0U)
            stringBuilder.Append("Subgroup:      ").Append(info.SubgroupMinSize).Append("..").Append(info.SubgroupMaxSize).Append(" lanes\n");
        WGPUFeatureName[] supportedFeatures = this.GetSupportedFeatures();
        stringBuilder.Append("Features:      ");
        if (supportedFeatures.Length == 0)
        {
            stringBuilder.Append("(none)");
        }
        else
        {
            for (int index = 0; index < supportedFeatures.Length; ++index)
            {
                if (index > 0)
                    stringBuilder.Append(", ");
                stringBuilder.Append((object)(WGPUFeatureName)(int)supportedFeatures[index]);
            }
        }
        stringBuilder.Append('\n');
        if (includeLimits)
        {
            stringBuilder.Append("Limits:\n");
            stringBuilder.Append("  maxTextureDimension2D=").Append(wgpuLimits.maxTextureDimension2D).Append('\n');
            stringBuilder.Append("  maxBufferSize=").Append(wgpuLimits.maxBufferSize).Append('\n');
            stringBuilder.Append("  maxBindGroups=").Append(wgpuLimits.maxBindGroups).Append('\n');
            stringBuilder.Append("  maxSampledTexturesPerShaderStage=").Append(wgpuLimits.maxSampledTexturesPerShaderStage).Append('\n');
            stringBuilder.Append("  maxStorageBuffersPerShaderStage=").Append(wgpuLimits.maxStorageBuffersPerShaderStage).Append('\n');
            stringBuilder.Append("  maxStorageTexturesPerShaderStage=").Append(wgpuLimits.maxStorageTexturesPerShaderStage).Append('\n');
            stringBuilder.Append("  maxUniformBufferBindingSize=").Append(wgpuLimits.maxUniformBufferBindingSize).Append('\n');
            stringBuilder.Append("  maxStorageBufferBindingSize=").Append(wgpuLimits.maxStorageBufferBindingSize).Append('\n');
            stringBuilder.Append("  maxComputeInvocationsPerWorkgroup=").Append(wgpuLimits.maxComputeInvocationsPerWorkgroup).Append('\n');
            stringBuilder.Append("  maxComputeWorkgroupSizeXYZ=").Append(wgpuLimits.maxComputeWorkgroupSizeX).Append('/').Append(wgpuLimits.maxComputeWorkgroupSizeY).Append('/').Append(wgpuLimits.maxComputeWorkgroupSizeZ).Append('\n');
            stringBuilder.Append("  maxColorAttachments=").Append(wgpuLimits.maxColorAttachments).Append('\n');
        }
        return stringBuilder.ToString();
    }

    public unsafe int GetSupportedFeatures(Span<WGPUFeatureName> destination)
    {
        this.ThrowIfDisposed();
        WGPUSupportedFeatures supportedFeatures = new WGPUSupportedFeatures();
        WGPU.wgpuAdapterGetFeatures(this._handle, &supportedFeatures);
        int featureCount = (int)supportedFeatures.featureCount;
        if (!destination.IsEmpty)
        {
            int num = Math.Min(featureCount, destination.Length);
            for (int index = 0; index < num; ++index)
            {
                destination[index] = (WGPUFeatureName)(((int*)supportedFeatures.features)[index]);
            }
        }
        WGPU.wgpuSupportedFeaturesFreeMembers(supportedFeatures);
        return featureCount;
    }

    private static unsafe string DecodeString(WGPUStringView view)
    {
        return view.data == null || view.length == 0 || view.length == WgpuDefaults.StrLen ? string.Empty : Encoding.UTF8.GetString((byte*)view.data, (int)view.length);
    }

    public unsafe WGPUNativeLimits GetNativeLimits()
    {
        this.ThrowIfDisposed();
        WGPUNativeLimits nativeLimits = new WGPUNativeLimits()
        {
            chain = new WGPUChainedStruct()
            {
                sType = (WGPUSType)196610 /*0x030002*/
            }
        };
        var __h0 = new WGPULimits()
        {
            nextInChain = (WGPUChainedStruct*)&nativeLimits
        };
        WGPU.wgpuAdapterGetLimits(this._handle, &__h0);
        return nativeLimits;
    }

    public unsafe void Dispose()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            return;
        WGPU.wgpuAdapterRelease(this._handle);
        this._handle = (WGPUAdapterImpl*)null;
        GC.SuppressFinalize((object)this);
    }

    unsafe ~Adapter()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            return;
        WGPU.wgpuAdapterRelease(this._handle);
    }

    private unsafe void ThrowIfDisposed()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(Adapter));
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void OnDevice(
      WGPURequestDeviceStatus status,
      WGPUDeviceImpl* device,
      WGPUStringView message,
      void* userdata1,
      void* userdata2)
    {
        ref Adapter.DeviceCallbackResult local = ref Unsafe.AsRef<Adapter.DeviceCallbackResult>(userdata1);
        local.Status = status;
        local.Device = device;
    }

    private struct DeviceCallbackResult
    {
        public WGPURequestDeviceStatus Status;
        public unsafe WGPUDeviceImpl* Device;
    }
}
