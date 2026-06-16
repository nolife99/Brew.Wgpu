using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace Brew.Wgpu;

public sealed class Instance : IDisposable
{
    private unsafe WGPUInstanceImpl* _handle;

    internal unsafe WGPUInstanceImpl* Handle => this._handle;

    private unsafe Instance(WGPUInstanceImpl* handle) => this._handle = handle;

    public static Instance Create() => Instance.Create(new InstanceDescriptor());

    public static unsafe Instance Create(in InstanceDescriptor descriptor)
    {
        if (descriptor.LogCallback != null)
        {
            WGPU.wgpuSetLogLevel(descriptor.LogLevel == null ? (WGPULogLevel)(object)2 : descriptor.LogLevel);
            WGPU.wgpuSetLogCallback(descriptor.LogCallback, descriptor.LogUserdata);
        }
        WGPUInstanceImpl* instance;
        if (descriptor.Flags != InstanceFlags.None || descriptor.Backends != InstanceBackends.None)
        {
            WGPUInstanceExtras wgpuInstanceExtras = new WGPUInstanceExtras()
            {
                chain = new WGPUChainedStruct()
                {
                    sType = (WGPUSType)196614 /*0x030006*/
                },
                flags = (ulong)descriptor.Flags,
                backends = (ulong)descriptor.Backends
            };
            var __h0 = new WGPUInstanceDescriptor()
            {
                nextInChain = (WGPUChainedStruct*)&wgpuInstanceExtras
            };
            instance = WGPU.wgpuCreateInstance(&__h0);
        }
        else
            instance = WGPU.wgpuCreateInstance((WGPUInstanceDescriptor*)null);
        return (IntPtr)instance != IntPtr.Zero ? new Instance(instance) : throw new ResourceCreationException(nameof(Instance), "wgpuCreateInstance");
    }

    public unsafe void ProcessEvents()
    {
        this.ThrowIfDisposed();
        WGPU.wgpuInstanceProcessEvents(this._handle);
    }

    public unsafe Adapter RequestAdapterBlocking(Surface compatibleSurface = default(Surface))
    {
        this.ThrowIfDisposed();
        Instance.AdapterCallbackResult adapterCallbackResult = new Instance.AdapterCallbackResult()
        {
            Status = (WGPURequestAdapterStatus)int.MaxValue
        };
        var __h1 = new WGPURequestAdapterOptions()
        {
            compatibleSurface = compatibleSurface.Handle
        };
        WGPU.wgpuInstanceRequestAdapter(this._handle, &__h1, new WGPURequestAdapterCallbackInfo()
        {
            mode = (WGPUCallbackMode)2,
            callback = &OnAdapter,
            userdata1 = Unsafe.AsPointer<Instance.AdapterCallbackResult>(ref adapterCallbackResult)
        });
        Async.PollUntilChanged<WGPURequestAdapterStatus>(this._handle, ref adapterCallbackResult.Status, (WGPURequestAdapterStatus)int.MaxValue);
        if ((int)adapterCallbackResult.Status != 1 || (IntPtr)adapterCallbackResult.Adapter == IntPtr.Zero)
            throw new AdapterRequestException(adapterCallbackResult.Status);
        return new Adapter(adapterCallbackResult.Adapter, this);
    }

    public unsafe Surface CreateSurface(SurfaceSource source)
    {
        this.ThrowIfDisposed();
        WGPUSurfaceImpl* surface;
        switch (source.Tag)
        {
            case SurfaceSource.Kind.WindowsHwnd:
                WGPUSurfaceSourceWindowsHWND sourceWindowsHwnd = new WGPUSurfaceSourceWindowsHWND()
                {
                    chain = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)5
                    },
                    hinstance = (void*)source.Handle0,
                    hwnd = (void*)source.Handle1
                };
                var __h2 = new WGPUSurfaceDescriptor()
                {
                    nextInChain = (WGPUChainedStruct*)&sourceWindowsHwnd
                };
                surface = WGPU.wgpuInstanceCreateSurface(this._handle, &__h2);
                break;
            case SurfaceSource.Kind.XlibWindow:
                WGPUSurfaceSourceXlibWindow sourceXlibWindow = new WGPUSurfaceSourceXlibWindow()
                {
                    chain = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)6
                    },
                    display = (void*)source.Handle0,
                    window = (ulong)(long)source.Handle1
                };
                var __h3 = new WGPUSurfaceDescriptor()
                {
                    nextInChain = (WGPUChainedStruct*)&sourceXlibWindow
                };
                surface = WGPU.wgpuInstanceCreateSurface(this._handle, &__h3);
                break;
            case SurfaceSource.Kind.WaylandSurface:
                WGPUSurfaceSourceWaylandSurface sourceWaylandSurface = new WGPUSurfaceSourceWaylandSurface()
                {
                    chain = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)7
                    },
                    display = (void*)source.Handle0,
                    surface = (void*)source.Handle1
                };
                var __h4 = new WGPUSurfaceDescriptor()
                {
                    nextInChain = (WGPUChainedStruct*)&sourceWaylandSurface
                };
                surface = WGPU.wgpuInstanceCreateSurface(this._handle, &__h4);
                break;
            case SurfaceSource.Kind.MetalLayer:
                WGPUSurfaceSourceMetalLayer sourceMetalLayer = new WGPUSurfaceSourceMetalLayer()
                {
                    chain = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)4
                    },
                    layer = (void*)source.Handle0
                };
                var __h5 = new WGPUSurfaceDescriptor()
                {
                    nextInChain = (WGPUChainedStruct*)&sourceMetalLayer
                };
                surface = WGPU.wgpuInstanceCreateSurface(this._handle, &__h5);
                break;
            case SurfaceSource.Kind.AndroidNativeWindow:
                WGPUSurfaceSourceAndroidNativeWindow androidNativeWindow = new WGPUSurfaceSourceAndroidNativeWindow()
                {
                    chain = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)8
                    },
                    window = (void*)source.Handle0
                };
                var __h6 = new WGPUSurfaceDescriptor()
                {
                    nextInChain = (WGPUChainedStruct*)&androidNativeWindow
                };
                surface = WGPU.wgpuInstanceCreateSurface(this._handle, &__h6);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(source), $"Unknown SurfaceSource.Kind: {source.Tag}");
        }
        return (IntPtr)surface != IntPtr.Zero ? new Surface(surface) : throw new ResourceCreationException("Surface", "wgpuInstanceCreateSurface");
    }

    public unsafe void Dispose()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            return;
        WGPU.wgpuInstanceRelease(this._handle);
        this._handle = (WGPUInstanceImpl*)null;
        GC.SuppressFinalize((object)this);
    }

    unsafe ~Instance()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            return;
        WGPU.wgpuInstanceRelease(this._handle);
    }

    private unsafe void ThrowIfDisposed()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(Instance));
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void OnAdapter(
      WGPURequestAdapterStatus status,
      WGPUAdapterImpl* adapter,
      WGPUStringView message,
      void* userdata1,
      void* userdata2)
    {
        ref Instance.AdapterCallbackResult local = ref Unsafe.AsRef<Instance.AdapterCallbackResult>(userdata1);
        local.Status = status;
        local.Adapter = adapter;
    }

    internal struct AdapterCallbackResult
    {
        public WGPURequestAdapterStatus Status;
        public unsafe WGPUAdapterImpl* Adapter;
    }
}
