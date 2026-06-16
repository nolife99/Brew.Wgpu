using System;

#nullable disable
namespace Brew.Wgpu;

public readonly struct SurfaceSource
{
    public readonly SurfaceSource.Kind Tag;
    public readonly IntPtr Handle0;
    public readonly IntPtr Handle1;

    private SurfaceSource(SurfaceSource.Kind tag, IntPtr h0, IntPtr h1)
    {
        this.Tag = tag;
        this.Handle0 = h0;
        this.Handle1 = h1;
    }

    public static SurfaceSource WindowsHwnd(IntPtr hinstance, IntPtr hwnd)
    {
        return new SurfaceSource(SurfaceSource.Kind.WindowsHwnd, hinstance, hwnd);
    }

    public static SurfaceSource XlibWindow(IntPtr display, ulong window)
    {
        return new SurfaceSource(SurfaceSource.Kind.XlibWindow, display, (IntPtr)(long)window);
    }

    public static SurfaceSource WaylandSurface(IntPtr display, IntPtr surface)
    {
        return new SurfaceSource(SurfaceSource.Kind.WaylandSurface, display, surface);
    }

    public static SurfaceSource MetalLayer(IntPtr layer)
    {
        return new SurfaceSource(SurfaceSource.Kind.MetalLayer, layer, IntPtr.Zero);
    }

    public static SurfaceSource AndroidNativeWindow(IntPtr window)
    {
        return new SurfaceSource(SurfaceSource.Kind.AndroidNativeWindow, window, IntPtr.Zero);
    }

    public enum Kind : byte
    {
        WindowsHwnd,
        XlibWindow,
        WaylandSurface,
        MetalLayer,
        AndroidNativeWindow,
    }
}
