using Brew.Wgpu.Native;
using System;

#nullable disable
namespace Brew.Wgpu.Internal;

internal static class WgpuDefaults
{
    public static readonly UIntPtr StrLen = new UIntPtr(18446744073709551615);

    public static unsafe WGPUStringView OptionalStringView(ReadOnlySpan<byte> bytes)
    {
        if (bytes.IsEmpty)
            return new WGPUStringView()
            {
                data = (sbyte*)null,
                length = WgpuDefaults.StrLen
            };
        fixed (byte* numPtr = &bytes.GetPinnableReference())
            return new WGPUStringView()
            {
                data = (sbyte*)numPtr,
                length = (UIntPtr)bytes.Length
            };
    }

    public static unsafe WGPUStringView OptionalStringView(byte* data, int length)
    {
        if ((IntPtr)data != IntPtr.Zero && length != 0)
            return new WGPUStringView()
            {
                data = (sbyte*)data,
                length = (UIntPtr)length
            };
        return new WGPUStringView()
        {
            data = (sbyte*)null,
            length = WgpuDefaults.StrLen
        };
    }
}
