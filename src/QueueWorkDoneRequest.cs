using Brew.Wgpu.Native;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Brew.Wgpu;

public readonly struct QueueWorkDoneRequest : IDisposable
{
    private readonly unsafe WGPUQueueWorkDoneStatus* _slot;

    internal unsafe QueueWorkDoneRequest(WGPUQueueWorkDoneStatus* slot) => this._slot = slot;

    public unsafe WGPUQueueWorkDoneStatus Status
    {
        get
        {
            return (IntPtr)this._slot == IntPtr.Zero ? (WGPUQueueWorkDoneStatus)int.MaxValue : (WGPUQueueWorkDoneStatus)(*(int*)this._slot);
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
