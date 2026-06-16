using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace Brew.Wgpu;

public sealed class Queue
{
    public unsafe WGPUQueueImpl* Handle { get; }

    internal unsafe Queue(WGPUQueueImpl* handle) => this.Handle = handle;

    public unsafe void Submit(CommandBuffer cmd)
    {
        var h = cmd.Handle;
        WGPU.wgpuQueueSubmit(this.Handle, 1, &h);
    }

    public unsafe void Submit(scoped ReadOnlySpan<CommandBuffer> commands)
    {
        ReadOnlySpan<IntPtr> readOnlySpan = MemoryMarshal.Cast<CommandBuffer, IntPtr>(commands);
        fixed (IntPtr* numPtr = &readOnlySpan.GetPinnableReference())
            WGPU.wgpuQueueSubmit(this.Handle, (UIntPtr)readOnlySpan.Length, (WGPUCommandBufferImpl**)numPtr);
    }

    public unsafe void WriteBuffer<T>(Buffer buffer, ulong bufferOffset, scoped ReadOnlySpan<T> data) where T : unmanaged
    {
        ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.AsBytes<T>(data);
        fixed (byte* numPtr = &readOnlySpan.GetPinnableReference())
            WGPU.wgpuQueueWriteBuffer(this.Handle, buffer.Handle, bufferOffset, (void*)numPtr, (UIntPtr)readOnlySpan.Length);
    }

    public unsafe void WriteTexture<T>(
        Texture destination,
        scoped ReadOnlySpan<T> data,
        uint bytesPerRow,
        uint rowsPerImage,
        scoped in WGPUExtent3D writeSize,
        uint mipLevel = 0,
        WGPUOrigin3D origin = default(WGPUOrigin3D))
        where T : unmanaged
    {
        ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.AsBytes<T>(data);
        WGPUTexelCopyTextureInfo texelCopyTextureInfo = new WGPUTexelCopyTextureInfo()
        {
            texture = destination.Handle,
            mipLevel = mipLevel,
            origin = origin
        };
        WGPUTexelCopyBufferLayout copyBufferLayout = new WGPUTexelCopyBufferLayout()
        {
            offset = 0,
            bytesPerRow = bytesPerRow,
            rowsPerImage = rowsPerImage
        };
        fixed (byte* numPtr = &readOnlySpan.GetPinnableReference())
        fixed (WGPUExtent3D* wgpuExtent3DPtr = &Unsafe.AsRef<WGPUExtent3D>(in writeSize))
            WGPU.wgpuQueueWriteTexture(this.Handle, &texelCopyTextureInfo, (void*)numPtr, (UIntPtr)readOnlySpan.Length, &copyBufferLayout, wgpuExtent3DPtr);
    }

    public unsafe WGPUQueueWorkDoneStatus OnSubmittedWorkDoneBlocking(Instance instance)
    {
        WGPUQueueWorkDoneStatus maxValue = (WGPUQueueWorkDoneStatus)int.MaxValue;
        WGPU.wgpuQueueOnSubmittedWorkDone(this.Handle, new WGPUQueueWorkDoneCallbackInfo()
        {
            mode = (WGPUCallbackMode)2,
            callback = &OnWorkDone,
            userdata1 = Unsafe.AsPointer(ref maxValue)
        });
        Async.PollUntilChanged<WGPUQueueWorkDoneStatus>(instance.Handle, ref maxValue, (WGPUQueueWorkDoneStatus)int.MaxValue);
        return maxValue;
    }

    public unsafe QueueWorkDoneRequest BeginOnSubmittedWorkDone()
    {
        WGPUQueueWorkDoneStatus* slot = (WGPUQueueWorkDoneStatus*)NativeMemory.Alloc(sizeof(WGPUQueueWorkDoneStatus));
        *(int*)slot = int.MaxValue;
        WGPU.wgpuQueueOnSubmittedWorkDone(this.Handle, new WGPUQueueWorkDoneCallbackInfo()
        {
            mode = (WGPUCallbackMode)2,
            callback = &OnWorkDone,
            userdata1 = (void*)slot
        });
        return new QueueWorkDoneRequest(slot);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void OnWorkDone(
        WGPUQueueWorkDoneStatus status,
        WGPUStringView message,
        void* userdata1,
        void* userdata2)
    {
        *(int*)userdata1 = (int)status;
    }
}
