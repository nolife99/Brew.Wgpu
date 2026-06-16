using Brew.Wgpu.Native;
using System;
using System.Runtime.CompilerServices;

#nullable enable
namespace Brew.Wgpu.Util;

public sealed class StagingBelt : IDisposable
{
    private readonly Device _device;
    private readonly ulong _chunkSize;
    private StagingBelt.Chunk[] _chunks;
    private int _chunkCount;
    private int[] _free;
    private int _freeCount;
    private int _active;
    private int[] _closed;
    private int _closedCount;
    private int[] _unmapped;
    private int _unmappedCount;
    private int[] _remapping;
    private int _remappingCount;
    private bool _disposed;

    public StagingBelt(Device device, ulong chunkSize)
    {
        ArgumentNullException.ThrowIfNull((object)device, nameof(device));
        if (chunkSize < 4UL)
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "chunkSize must be >= 4.");
        this._chunkSize = chunkSize + 3UL & 18446744073709551612UL;
        this._device = device;
        this._chunks = new StagingBelt.Chunk[4];
        this._free = new int[4];
        this._closed = new int[4];
        this._unmapped = new int[4];
        this._remapping = new int[4];
        this._active = -1;
    }

    public ulong ChunkSize => this._chunkSize;

    public int TotalChunks => this._chunkCount;

    public int FreeChunks => this._freeCount;

    public int InFlightChunks
    {
        get
        {
            return (this._active >= 0 ? 1 : 0) + this._closedCount + this._unmappedCount + this._remappingCount;
        }
    }

    public ulong TotalBytes
    {
        get
        {
            ulong totalBytes = 0;
            for (int index = 0; index < this._chunkCount; ++index)
                totalBytes += this._chunks[index].Size;
            return totalBytes;
        }
    }

    public unsafe Span<byte> WriteBuffer(
      CommandEncoder encoder,
      Brew.Wgpu.Buffer target,
      ulong offset,
      ulong size)
    {
        this.ThrowIfDisposed();
        if (size == 0UL)
            throw new ArgumentOutOfRangeException(nameof(size), "size must be > 0.");
        if (((long)size & 3L) != 0L)
            throw new ArgumentException("size must be 4-byte aligned.", nameof(size));
        if (((long)offset & 3L) != 0L)
            throw new ArgumentException("offset must be 4-byte aligned.", nameof(offset));
        if (target.IsNull)
            throw new ArgumentException("target buffer is null.", nameof(target));
        (int idx, ulong num) = this.Reserve(size);
        ref StagingBelt.Chunk local = ref this._chunks[idx];
        encoder.CopyBufferToBuffer(local.Buffer, num, target, offset, size);
        return new Span<byte>((void*)(local.MappedPtr + num), (int)size);
    }

    public unsafe void WriteTexture(
      CommandEncoder encoder,
      Texture target,
      scoped in WGPUExtent3D writeSize,
      WGPUTextureFormat format,
      scoped ReadOnlySpan<byte> packedData,
      uint mipLevel = 0,
      WGPUOrigin3D origin = default(WGPUOrigin3D),
      WGPUTextureAspect aspect = (WGPUTextureAspect)1)
    {
        this.ThrowIfDisposed();
        if (target.IsNull)
            throw new ArgumentException("target texture is null.", nameof(target));
        if (packedData.IsEmpty)
            throw new ArgumentException("packedData must be non-empty.", nameof(packedData));
        uint num1 = format.BytesPerBlock();
        if (num1 == 0U)
            throw new ArgumentException($"Format {format} has no directly copyable block size; pick an aspect with a defined byte size.", nameof(format));
        (uint w, uint h) = format.BlockDimensions();
        int num2 = (int)((uint)((int)writeSize.width + (int)w - 1) / w);
        uint num3 = (uint)((int)writeSize.height + (int)h - 1) / h;
        uint num4 = writeSize.depthOrArrayLayers == 0U ? 1U : writeSize.depthOrArrayLayers;
        int num5 = (int)num1;
        uint sourceBytesToCopy1 = (uint)(num2 * num5);
        uint destinationSizeInBytes1 = (uint)((int)sourceBytesToCopy1 + (int)byte.MaxValue & -256);
        ulong sourceBytesToCopy2 = (ulong)sourceBytesToCopy1 * (ulong)num3 * (ulong)num4;
        if ((long)packedData.Length != (long)sourceBytesToCopy2)
            throw new ArgumentException($"packedData.Length ({packedData.Length}) does not match expected packed size ({sourceBytesToCopy2}) for extent {writeSize.width}×{writeSize.height}×{num4} format {format}.", nameof(packedData));
        ulong destinationSizeInBytes2 = (ulong)destinationSizeInBytes1 * (ulong)num3 * (ulong)num4;
        (int idx, ulong offset) = this.Reserve(destinationSizeInBytes2 + 3UL & 18446744073709551612UL);
        ref StagingBelt.Chunk local = ref this._chunks[idx];
        byte* destination = local.MappedPtr + offset;
        fixed (byte* source = &packedData.GetPinnableReference())
        {
            uint num6 = num3 * num4;
            if ((int)sourceBytesToCopy1 == (int)destinationSizeInBytes1)
            {
                System.Buffer.MemoryCopy((void*)source, (void*)destination, destinationSizeInBytes2, sourceBytesToCopy2);
            }
            else
            {
                for (uint index = 0; index < num6; ++index)
                    System.Buffer.MemoryCopy((void*)(source + index * sourceBytesToCopy1), (void*)(destination + index * destinationSizeInBytes1), (long)destinationSizeInBytes1, (long)sourceBytesToCopy1);
            }
        }
        WGPUTexelCopyBufferInfo texelCopyBufferInfo = new WGPUTexelCopyBufferInfo()
        {
            buffer = local.Buffer.Handle,
            layout = new WGPUTexelCopyBufferLayout()
            {
                offset = offset,
                bytesPerRow = destinationSizeInBytes1,
                rowsPerImage = num3
            }
        };
        WGPUTexelCopyTextureInfo texelCopyTextureInfo = new WGPUTexelCopyTextureInfo()
        {
            texture = target.Handle,
            mipLevel = mipLevel,
            origin = origin,
            aspect = aspect
        };
        fixed (WGPUExtent3D* wgpuExtent3DPtr = &Unsafe.AsRef<WGPUExtent3D>(in writeSize))
            WGPU.wgpuCommandEncoderCopyBufferToTexture(encoder.Handle, &texelCopyBufferInfo, &texelCopyTextureInfo, wgpuExtent3DPtr);
    }

    public void Finish()
    {
        this.ThrowIfDisposed();
        if (this._active >= 0)
        {
            this.UnmapChunk(this._active);
            this.PushUnmapped(this._active);
            this._active = -1;
        }
        for (int index = 0; index < this._closedCount; ++index)
        {
            int idx = this._closed[index];
            this.UnmapChunk(idx);
            this.PushUnmapped(idx);
        }
        this._closedCount = 0;
    }

    public void Recall()
    {
        this.ThrowIfDisposed();
        for (int index = 0; index < this._unmappedCount; ++index)
        {
            int idx = this._unmapped[index];
            ref StagingBelt.Chunk local = ref this._chunks[idx];
            local.MapReq = local.Buffer.BeginMap(MapMode.Write, 0UL, (UIntPtr)local.Size);
            this.PushRemapping(idx);
        }
        this._unmappedCount = 0;
    }

    private (int idx, ulong offset) Reserve(ulong size)
    {
        if (this._active >= 0)
        {
            ref StagingBelt.Chunk local = ref this._chunks[this._active];
            ulong num = local.Offset + 3UL & 18446744073709551612UL;
            if (num + size <= local.Size)
            {
                local.Offset = num + size;
                return (this._active, num);
            }
            this.PushClosed(this._active);
            this._active = -1;
        }
        this.DrainCompleted();
        int index1 = -1;
        for (int index2 = 0; index2 < this._freeCount; ++index2)
        {
            if (this._chunks[this._free[index2]].Size >= size)
            {
                index1 = index2;
                break;
            }
        }
        int index3;
        if (index1 >= 0)
        {
            index3 = this._free[index1];
            this._free[index1] = this._free[--this._freeCount];
        }
        else
            index3 = this.AllocateChunk(size);
        this._active = index3;
        this._chunks[index3].Offset = size;
        return (index3, 0UL);
    }

    private unsafe int AllocateChunk(ulong minSize)
    {
        ulong num = minSize > this._chunkSize ? (ulong)((long)minSize + 3L & -4L) : this._chunkSize;
        Brew.Wgpu.Buffer buffer = this._device.CreateBuffer(new BufferDescriptor()
        {
            Size = num,
            Usage = BufferUsage.MapWrite | BufferUsage.CopySrc,
            MappedAtCreation = true
        });
        void* mappedRange = WGPU.wgpuBufferGetMappedRange(buffer.Handle, UIntPtr.Zero, (UIntPtr)num);
        if ((IntPtr)mappedRange == IntPtr.Zero)
        {
            buffer.Dispose();
            throw new InvalidOperationException("wgpuBufferGetMappedRange returned null for a freshly created staging chunk.");
        }
        this.EnsureChunksCapacity(this._chunkCount + 1);
        int index = this._chunkCount++;
        this._chunks[index] = new StagingBelt.Chunk()
        {
            Buffer = buffer,
            Size = num,
            Offset = 0UL,
            MappedPtr = (byte*)mappedRange,
            MapReq = new BufferMapRequest()
        };
        return index;
    }

    public unsafe void DrainCompleted()
    {
        int num = 0;
        for (int index1 = 0; index1 < this._remappingCount; ++index1)
        {
            int idx = this._remapping[index1];
            ref StagingBelt.Chunk local = ref this._chunks[idx];
            if (local.MapReq.IsComplete)
            {
                WGPUMapAsyncStatus status = local.MapReq.Status;
                local.MapReq.Dispose();
                local.MapReq = new BufferMapRequest();
                if ((int)status == 1)
                {
                    void* mappedRange = WGPU.wgpuBufferGetMappedRange(local.Buffer.Handle, UIntPtr.Zero, (UIntPtr)local.Size);
                    if ((IntPtr)mappedRange == IntPtr.Zero)
                    {
                        for (int index2 = index1 + 1; index2 < this._remappingCount; ++index2)
                            this._remapping[num++] = this._remapping[index2];
                        this._remappingCount = num;
                        throw new InvalidOperationException("wgpuBufferGetMappedRange returned null after a successful remap.");
                    }
                    local.MappedPtr = (byte*)mappedRange;
                    local.Offset = 0UL;
                    this.PushFree(idx);
                }
                else
                {
                    for (int index3 = index1 + 1; index3 < this._remappingCount; ++index3)
                        this._remapping[num++] = this._remapping[index3];
                    this._remappingCount = num;
                    throw new InvalidOperationException($"StagingBelt chunk remap failed with status {status}.");
                }
            }
            else
                this._remapping[num++] = idx;
        }
        this._remappingCount = num;
    }

    private unsafe void UnmapChunk(int idx)
    {
        ref StagingBelt.Chunk local = ref this._chunks[idx];
        if ((IntPtr)local.MappedPtr == IntPtr.Zero)
            return;
        local.Buffer.Unmap();
        local.MappedPtr = (byte*)null;
    }

    private void PushFree(int idx)
    {
        if (this._freeCount == this._free.Length)
            Array.Resize<int>(ref this._free, this._free.Length * 2);
        this._free[this._freeCount++] = idx;
    }

    private void PushClosed(int idx)
    {
        if (this._closedCount == this._closed.Length)
            Array.Resize<int>(ref this._closed, this._closed.Length * 2);
        this._closed[this._closedCount++] = idx;
    }

    private void PushUnmapped(int idx)
    {
        if (this._unmappedCount == this._unmapped.Length)
            Array.Resize<int>(ref this._unmapped, this._unmapped.Length * 2);
        this._unmapped[this._unmappedCount++] = idx;
    }

    private void PushRemapping(int idx)
    {
        if (this._remappingCount == this._remapping.Length)
            Array.Resize<int>(ref this._remapping, this._remapping.Length * 2);
        this._remapping[this._remappingCount++] = idx;
    }

    private void EnsureChunksCapacity(int required)
    {
        if (this._chunks.Length >= required)
            return;
        int length = this._chunks.Length;
        while (length < required)
            length *= 2;
        Array.Resize<StagingBelt.Chunk>(ref this._chunks, length);
    }

    private void ThrowIfDisposed()
    {
        if (this._disposed)
            throw new ObjectDisposedException(nameof(StagingBelt));
    }

    public unsafe void Dispose()
    {
        if (this._disposed)
            return;
        this._disposed = true;
        for (int index = 0; index < this._chunkCount; ++index)
        {
            ref StagingBelt.Chunk local = ref this._chunks[index];
            local.MapReq.Dispose();
            local.MapReq = new BufferMapRequest();
            if ((IntPtr)local.MappedPtr != IntPtr.Zero)
            {
                local.Buffer.Unmap();
                local.MappedPtr = (byte*)null;
            }
            local.Buffer.Dispose();
        }
        this._chunkCount = 0;
        this._freeCount = 0;
        this._closedCount = 0;
        this._unmappedCount = 0;
        this._remappingCount = 0;
        this._active = -1;
    }

    private struct Chunk
    {
        public Brew.Wgpu.Buffer Buffer;
        public ulong Size;
        public ulong Offset;
        public unsafe byte* MappedPtr;
        public BufferMapRequest MapReq;
    }
}
