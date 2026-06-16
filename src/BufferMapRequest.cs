using Brew.Wgpu.Native;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Brew.Wgpu;

public unsafe readonly struct BufferMapRequest : IDisposable
{
    readonly WGPUMapAsyncStatus* _slot;
    internal BufferMapRequest(WGPUMapAsyncStatus* slot) => _slot = slot;

    public WGPUMapAsyncStatus Status => _slot == null ? (WGPUMapAsyncStatus)int.MaxValue : *_slot;

    public bool IsComplete => (int)Status != int.MaxValue;
    public bool IsSuccess => (int)Status == 1;

    public void Dispose()
    {
        if ((IntPtr)_slot == IntPtr.Zero)
            return;
        NativeMemory.Free(_slot);
    }
}
