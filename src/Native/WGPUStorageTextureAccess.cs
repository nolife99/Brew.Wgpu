namespace Brew.Wgpu.Native
{
    [NativeTypeName("unsigned int")]
    public enum WGPUStorageTextureAccess : uint
    {
        BindingNotUsed = 0x00000000,
        Undefined = 0x00000001,
        WriteOnly = 0x00000002,
        ReadOnly = 0x00000003,
        ReadWrite = 0x00000004,
        Force32 = 0x7FFFFFFF,
    }
}
