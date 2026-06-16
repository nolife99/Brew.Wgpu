using Brew.Wgpu.Native;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable enable
namespace Brew.Wgpu.Internal;

internal static class Async
{
    public static unsafe void PollUntilChanged<TStatus>(
      WGPUInstanceImpl* instance,
      ref TStatus status,
      TStatus sentinel,
      int timeoutMs = 5000)
      where TStatus : unmanaged, Enum
    {
        int num1 = Unsafe.As<TStatus, int>(ref sentinel);
        long num2 = Environment.TickCount64 + (long)timeoutMs;
        SpinWait spinWait = new SpinWait();
        while (Unsafe.As<TStatus, int>(ref status) == num1)
        {
            WGPU.wgpuInstanceProcessEvents(instance);
            if (Unsafe.As<TStatus, int>(ref status) != num1)
                break;
            if (Environment.TickCount64 >= num2)
                throw new TimeoutException($"wgpu callback did not fire within {timeoutMs} ms.");
            spinWait.SpinOnce(-1);
        }
    }
}
