using Brew.Wgpu.Native;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Brew.Wgpu;

public readonly struct BufferMapRequest : IDisposable
{
    private readonly unsafe WGPUMapAsyncStatus* _slot;

    internal unsafe BufferMapRequest(WGPUMapAsyncStatus* slot) => this._slot = slot;

    public unsafe WGPUMapAsyncStatus Status
    {
        get
        {
            return (IntPtr)this._slot == IntPtr.Zero ? (WGPUMapAsyncStatus)int.MaxValue : (WGPUMapAsyncStatus)(*(int*)this._slot);
        }
    }

    public bool IsComplete => (int)this.Status != int.MaxValue;

    public bool IsSuccess => (int)this.Status == 1;

    public unsafe void Dispose()
    {
        if ((IntPtr)this._slot == IntPtr.Zero)
            return;
        NativeMemory.Free((void*)this._slot);
    }
}
