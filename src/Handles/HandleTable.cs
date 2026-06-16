using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Brew.Wgpu.Handles;

/// <summary>
/// A generational slot table — one static instance per native resource kind
/// (<typeparamref name="T"/> is the opaque <c>WGPU*Impl</c> pointee).
///
/// <para>
/// It backs the value-type resource handles. Every handle is just a
/// <c>{slot, generation}</c> pair, so copying a handle is free and it can live
/// in fields, arrays, dictionaries, and cross threads. Releasing a resource
/// bumps that slot's generation, which invalidates <b>every</b> existing copy at
/// once — the same "dispose invalidates all aliases" guarantee a class gets from
/// reference identity, but with no per-handle heap allocation.
/// </para>
///
/// <para>Safety properties:</para>
/// <list type="bullet">
///   <item><b>Double-free safe:</b> <see cref="Retire"/> returns the pointer only
///   the first time; later calls (including on copies) see the bumped generation
///   and no-op.</item>
///   <item><b>Use-after-free safe:</b> <see cref="Resolve"/> on a stale handle
///   returns <c>null</c>, so the wrapper throws instead of dereferencing freed
///   memory.</item>
///   <item><b>ABA safe:</b> the generation distinguishes a reused slot from the
///   handle that previously owned it, which a raw-pointer "live set" cannot.</item>
/// </list>
/// </summary>
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

    /// <summary>Register a freshly-created native pointer and return its handle key.</summary>
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
                    // Grow by hand and release-publish the new slab. The lock-free Resolve below
                    // indexes without a bounds check, which is only sound if every reader observes
                    // a slab at least large enough for any slot we have ever issued — the Volatile
                    // write here is that release, paired with the Volatile read in Resolve.
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

    /// <summary>
    /// Resolve a handle to its live pointer, or <c>null</c> if it has been retired
    /// (stale generation). Lock-free fast path: correct as long as a resource is
    /// not retired on one thread while concurrently in use on another — the same
    /// contract a raw native pointer already carries.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Resolve(int slot, uint gen)
    {
        Slot[] slots = Volatile.Read(ref _slots);  // acquire; pairs with the release in the grow path
        // Slots are issued only by Register (always < length at issue) and the slab only ever grows,
        // so any handle we hand out — and the default {0,0} handle — is in range. Trust ourselves and
        // skip the bounds check; the assert catches a fabricated/corrupted handle in Debug builds.
        Debug.Assert((uint)slot < (uint)slots.Length, "handle slot out of range — fabricated or corrupted handle");
        ref Slot s = ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(slots), (nint)(uint)slot);
        return s.Gen == gen ? s.Ptr : null;
    }

    /// <summary>
    /// Retire a handle exactly once. Returns the pointer the caller must hand to
    /// the matching <c>wgpu*Release</c>, or <c>null</c> if the handle was already
    /// retired or is stale — so a double-dispose (or disposing a copy) is a safe
    /// no-op rather than a native refcount underflow.
    /// </summary>
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
