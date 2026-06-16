namespace Brew.Wgpu.Native
{
    public partial struct WGPUFutureWaitInfo
    {
        public WGPUFuture future;

        [NativeTypeName("WGPUBool")]
        public uint completed;
    }
}
