// DisableRuntimeMarshalling requires net7.0+. On net6.0 it is intentionally
// absent: the generated structs are blittable, so the default runtime marshaller
// takes the no-op fast path (no manual marshalling needed). On net7.0+ the
// attribute restores the enforced, stub-free path.
#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;

[assembly: DisableRuntimeMarshalling]
#endif
