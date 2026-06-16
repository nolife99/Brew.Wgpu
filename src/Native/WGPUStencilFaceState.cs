namespace Brew.Wgpu.Native
{
    public partial struct WGPUStencilFaceState
    {
        public WGPUCompareFunction compare;

        public WGPUStencilOperation failOp;

        public WGPUStencilOperation depthFailOp;

        public WGPUStencilOperation passOp;
    }
}
