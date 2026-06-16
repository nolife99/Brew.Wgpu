using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Brew.Wgpu.Handles;

internal static unsafe class HandleTable<T> where T : unmanaged
{
    private struct Slot
    {
        public T* Ptr;     // null == free
        public uint Gen;   // 0 is reserved for the null/default handle; live gens are >= 1
    }

    private static Slot[] _slots = new Slot[64];
    private static int[] _freeList = new int[64];
    private static int _freeCount;
    private static int _highWater;             // count of slots ever allocated
#if NET9_0_OR_GREATER
    private static readonly Lock _gate = new();   // net9+: dedicated Lock type (faster than Monitor)
#else
    private static readonly object _gate = new(); // net6/net8: Monitor via the object
#endif

    public static (int slot, uint gen) Register(T* ptr)
    {
        lock (_gate)
        {
            int slot;
            if (_freeCount > 0)
            {
                slot = _freeList[--_freeCount];
            }
            else
            {
                if (_highWater == _slots.Length)
                {
                    Slot[] bigger = new Slot[_slots.Length * 2];
                    Array.Copy(_slots, bigger, _highWater);
                    Volatile.Write(ref _slots, bigger);
                }
                slot = _highWater++;
                _slots[slot].Gen = 1;          // first generation for a brand-new slot
            }
            _slots[slot].Ptr = ptr;
            return (slot, _slots[slot].Gen);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Resolve(int slot, uint gen)
    {
        Slot[] slots = Volatile.Read(ref _slots);
        Debug.Assert((uint)slot < (uint)slots.Length, "handle slot out of range — fabricated or corrupted handle");
        ref Slot s = ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(slots), (nint)(uint)slot);
        return s.Gen == gen ? s.Ptr : null;
    }

    public static T* Retire(int slot, uint gen)
    {
        lock (_gate)
        {
            if ((uint)slot >= (uint)_slots.Length)
                return null;
            ref Slot s = ref _slots[slot];
            if (s.Ptr == null || s.Gen != gen)
                return null;                       // double-free / stale copy

            T* p = s.Ptr;
            s.Ptr = null;
            unchecked { s.Gen++; }                 // invalidate every existing copy
            if (s.Gen == 0) s.Gen = 1;             // skip the reserved null generation on wrap

            if (_freeCount == _freeList.Length)
                Array.Resize(ref _freeList, _freeList.Length * 2);
            _freeList[_freeCount++] = slot;
            return p;
        }
    }
}
