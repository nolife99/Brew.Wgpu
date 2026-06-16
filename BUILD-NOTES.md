# Build notes

## Status

| Layer | State |
|-------|-------|
| `.github/workflows/` | Complete. `build-wgpu-native.yml` (matrix → `runtimes/`), reusable; `release.yml` (manual pack + push). |
| `src/Native/` (212 files) | **Compiles.** Regenerated with ClangSharp; types are byte-correct (`uint64_t→ulong`, `size_t→nuint`, `WGPUFlags`-aliases→`ulong`); the memory-hints fields are present. |
| `src/Handles/` + handle types | **Compiles.** Generational, double-free-safe `{slot, generation}` handles. |
| `src/` wrapper (Device, Queue, Surface, …) | **Compiles.** Recovered from lossy ILSpy decompilation; all mechanical artifacts fixed (details below). |

The whole project builds clean (**0 errors**) on `net6.0` and `net8.0` and packs to
`.nupkg`/`.snupkg`. Target floor is **`net6.0`** (multi-targeting `net6.0;net8.0`): the generated
structs are blittable (`WGPUBool→uint`, no `bool`/`char`/`[MarshalAs]` in interop), so default
runtime marshalling is the no-op fast path — no manual marshalling. `[assembly: DisableRuntimeMarshalling]`
is guarded `#if NET7_0_OR_GREATER`, so it's absent on `net6.0` (where it isn't available) and present
on `net7.0+` (where it restores the enforced, stub-free path). There are 24 benign compiler warnings
(see *Residual warnings* below).

## Regenerating the bindings

Run on **Windows** (`tools/generate-bindings/generate.ps1`). The Windows clang target maps
`uint64_t→ulong` and `size_t→nuint` correctly (the Linux target mis-maps `uint64_t→nuint` *and*
the `WGPUFlags` aliases — `WGPUBufferUsage`, `WGPUTextureUsage`, `WGPUMapMode`, `WGPUShaderStage`,
`WGPUColorWriteMask`, `WGPUInstanceFlag/Backend` — to `nuint`, which corrupts layout on 32-bit;
`nuint` is correct only for `size_t`/`uintptr_t`). Notes:

- Generated **without** `generate-macro-bindings` — the `WGPU_*_INIT` convenience macros it emits
  reference `__builtin_nanf` and don't compile. The flag constants (`WGPUBufferUsage_*`, …) survive
  as plain `static const`.
- ClangSharp does **not** carry the headers' doxygen into the bindings (verified). Document the thin
  wrapper by hand, or post-process if you want XML docs on the raw layer.
- Pin the tool to `18.1.0.4` (newer versions failed to install as a dotnet tool). On Linux CI the tool
  ships only Windows natives — `generate.sh` fetches the linux-x64 `libClangSharp`/`libclang` and adds
  the `libclang.so.18.1` soname symlink.

## Wrapper recovery (now compiles)

The ILSpy output dropped enum-member names to ints, dropped ClangSharp's `WGPU…_` member
prefixes, rendered `WGPUBool` fields as `(uint)bool` casts, and produced a few illegal
constructs. All were mechanical; what was applied:

- **bool→`uint`** — `(uint)someBool` → `someBool ? 1u : 0u` (WGPUBool fields).
- **enum-vs-int** — `status != 1` → `(int)status != 1`; same for the `Is*`/`Needs*` helpers.
- **`(Enum) *(int*)ptr` cast/multiply ambiguity** — the compiler read `(Enum) * (int*)…` as a
  multiply; parenthesised the deref: `(Enum)(*(int*)ptr)`.
- **feature enumeration** — `*(int*)((IntPtr)p + (IntPtr)i * 4)` (illegal `IntPtr` arithmetic) →
  `((int*)p)[i]`.
- **ClangSharp enum-prefix names** — e.g. `WGPUCallbackMode.WGPUCallbackMode_AllowProcessEvents`.
- **readonly `in` extents** — `Unsafe.AsRef<T>(ref size)` on a `scoped in` param → `Unsafe.AsRef(in size)`.
- **`DecodeString`** — dropped illegal `(IntPtr)nuint` casts; compare the pointer to `null` and the
  `size_t` length to `0` / `WGPU_STRLEN`.
- **`&cmd.Handle`** — can't take the address of a property; copy to a local first.
- **`Buffer`** — given the same registering `internal unsafe Buffer(WGPUBufferImpl*)` ctor the other
  11 handles have, so `new Buffer(ptr)` compiles and registers into the table.
- **binding type fixes** — the `WGPUFlags` aliases were `nuint` (Linux mis-map) → `ulong`; three
  `size_t` params that shared a source line with a 64-bit param had been dragged to `ulong` → back
  to `nuint`.

### API drift (wgpu-native v29)

The wrapper predates v29 and referenced `WGPUPipelineLayoutExtras`, which the bundled v29 headers
no longer expose. `Device.CreatePipelineLayout(layouts, pushConstantBytes)` now builds a plain
`WGPUPipelineLayoutDescriptor` and **throws `NotSupportedException` if `pushConstantBytes > 0`**.
The no-push-constant path (and the `CreatePipelineLayout(layouts)` overload) work normally.
Regenerate the bindings against a wgpu-native that defines the struct to restore push constants.

### Residual warnings (24, benign)

- **CS0472 (×20)** — the decompiler rendered nullable enums (`WGPUPrimitiveTopology?`,
  `WGPUVertexStepMode?`, `WGPULogLevel?`) as their non-nullable underlying type, so the original
  `== null` default-guards are now always-false (dead branches). They compile and behave as the
  decompiled logic dictates, but the lost nullable semantics are worth a review pass if those
  defaults matter to you.
- **CS0219 / CS0162 (×4)** — one unused local and one unreachable branch, both harmless.

## MipmapGenerator (reconstructed)

`src/Util/MipmapGenerator.cs` + `MipmapGenerator.wgsl.cs` — ILSpy originally left these uncompilable:
the five `stackalloc` spans came out as `// Unable to render the statement` with bogus pointer locals,
and the WGSL source plus the `vs`/`fs` entry-point names were stored as `<PrivateImplementationDetails>`
RVA byte blobs. Both are now reconstructed and compiled: stackallocs restored, the WGSL rewritten as a
`u8` literal (full-screen-triangle linear-downsample blit), entry points as `"vs"u8`/`"fs"u8`, and the
bare enum casts (`(WGPUFilterMode)2`, …) replaced with named members. The logic — a per-array-layer,
top-down render-pass mip pyramid that samples level N-1 into level N — was reviewed and is correct; the
magic values were all verified faithful (2D, filterable-Float, Linear, ClampToEdge, Clear/Store).

## Native binaries / memory hints

Binaries are built fresh by `release.yml`. To get the memory-hints behaviour, point `wgpu_repo`/`wgpu_ref`
at your patched fork (the `wgpu.h` enum + `WGPUDeviceExtras` fields **and** the `conv.rs` mapping).
Upstream binaries ignore the hint.
