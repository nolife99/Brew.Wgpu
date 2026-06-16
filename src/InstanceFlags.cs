using System;

#nullable disable
namespace Brew.Wgpu;

[Flags]
public enum InstanceFlags : ulong
{
    None = 0,
    Debug = 1,
    Validation = 2,
    DiscardHalLabels = 4,
    AllowNoncompliantAdapter = 8,
    GpuBasedValidation = 16, // 0x0000000000000010
    ValidationIndirectCall = 32, // 0x0000000000000020
    AutomaticTimestampNormalization = 64, // 0x0000000000000040
    DevDefault = Validation | Debug, // 0x0000000000000003
}
