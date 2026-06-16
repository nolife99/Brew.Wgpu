using Brew.Wgpu.Native;
using System;
using System.Collections.Generic;

#nullable enable
namespace Brew.Wgpu.Util;

/// <summary>
/// Generates a 2D / 2D-array texture's mip chain on the GPU. For each array layer
/// it walks the chain top-down, rendering a full-screen triangle that samples the
/// previous (larger) mip with a linear sampler into the next (smaller) mip.
/// <para>
/// The base level must already be populated, and the texture must be created with
/// render-attachment usage and a filterable-float color format.
/// </para>
/// </summary>
public sealed class MipmapGenerator : IDisposable
{
    private readonly Device _device;
    private Sampler _sampler;
    private BindGroupLayout _bindGroupLayout;
    private PipelineLayout _pipelineLayout;
    private ShaderModule _shaderModule;
    // The color-target format is baked into a render pipeline, so cache one per format.
    private readonly Dictionary<WGPUTextureFormat, RenderPipeline> _pipelines = new();
    private bool _disposed;

    public unsafe MipmapGenerator(Device device)
    {
        ArgumentNullException.ThrowIfNull(device);
        _device = device;

        _sampler = device.CreateSampler(new SamplerDescriptor
        {
            AddressModeU = WGPUAddressMode.ClampToEdge,
            AddressModeV = WGPUAddressMode.ClampToEdge,
            AddressModeW = WGPUAddressMode.ClampToEdge,
            MinFilter = WGPUFilterMode.Linear,
            MagFilter = WGPUFilterMode.Linear,
            MipmapFilter = WGPUMipmapFilterMode.Nearest
        });

        Span<BindGroupLayoutEntry> layoutEntries = stackalloc BindGroupLayoutEntry[2];
        layoutEntries[0] = BindGroupLayoutEntry.Texture(0U, ShaderStage.Fragment);
        layoutEntries[1] = BindGroupLayoutEntry.Sampler(1U, ShaderStage.Fragment);
        _bindGroupLayout = device.CreateBindGroupLayout(layoutEntries);

        Span<BindGroupLayout> layouts = stackalloc BindGroupLayout[1];
        layouts[0] = _bindGroupLayout;
        _pipelineLayout = device.CreatePipelineLayout(layouts);

        _shaderModule = device.CreateShaderModule(new ShaderModuleDescriptor
        {
            Source = ShaderSource.FromWgsl(MipmapGeneratorWgsl.Source)
        });
    }

    /// <summary>
    /// Records the mip-generation passes into <paramref name="encoder"/>. Generates
    /// levels <c>baseMipLevel + 1 .. baseMipLevel + mipLevelCount - 1</c> from the
    /// level below each. <paramref name="mipLevelCount"/> of 0 means "all remaining
    /// levels from <paramref name="baseMipLevel"/>".
    /// </summary>
    public unsafe void Generate(
        CommandEncoder encoder,
        Texture texture,
        uint baseMipLevel = 0,
        uint mipLevelCount = 0)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MipmapGenerator));
        if (texture.IsNull)
            throw new ArgumentException("texture is null.", nameof(texture));
        if (texture.Dimension != WGPUTextureDimension._2D)
            throw new NotSupportedException(
                $"MipmapGenerator only supports 2D / 2D-array textures (got {texture.Dimension}).");

        WGPUTextureFormat format = texture.Format;
        if (format.DefaultSampleType() != WGPUTextureSampleType.Float)
            throw new NotSupportedException(
                $"MipmapGenerator only supports filterable-float color formats (got {format}). " +
                "Integer, depth, stencil, and unfilterable-float formats need a different shader/sampler combo.");

        uint levels = texture.MipLevelCount;
        if (baseMipLevel >= levels)
            throw new ArgumentOutOfRangeException(nameof(baseMipLevel),
                $"baseMipLevel ({baseMipLevel}) must be < texture.MipLevelCount ({levels}).");

        uint count = mipLevelCount == 0U ? levels - baseMipLevel : mipLevelCount;
        if (count > levels - baseMipLevel)
            throw new ArgumentOutOfRangeException(nameof(mipLevelCount),
                $"baseMipLevel + mipLevelCount ({baseMipLevel + mipLevelCount}) exceeds texture.MipLevelCount ({levels}).");
        if (count <= 1U)
            return;

        uint layerCount = texture.DepthOrArrayLayers;
        RenderPipeline pipeline = GetOrCreatePipeline(format);

        // Reused every iteration; the spans are consumed synchronously by the calls below.
        Span<BindGroupEntry> bindEntries = stackalloc BindGroupEntry[2];
        Span<ColorAttachment> colorAttachments = stackalloc ColorAttachment[1];

        for (uint layer = 0; layer < layerCount; ++layer)
        {
            for (uint level = baseMipLevel + 1U; level < baseMipLevel + count; ++level)
            {
                TextureViewDescriptor srcDesc = new()
                {
                    Dimension = WGPUTextureViewDimension._2D,
                    BaseMipLevel = level - 1U,   // source = the larger level above
                    MipLevelCount = 1U,
                    BaseArrayLayer = layer,
                    ArrayLayerCount = 1U
                };
                TextureView srcView = texture.CreateView(in srcDesc);

                TextureViewDescriptor dstDesc = new()
                {
                    Dimension = WGPUTextureViewDimension._2D,
                    BaseMipLevel = level,        // target = the level being written
                    MipLevelCount = 1U,
                    BaseArrayLayer = layer,
                    ArrayLayerCount = 1U
                };
                TextureView dstView = texture.CreateView(in dstDesc);

                bindEntries[0] = BindGroupEntry.TextureView(0U, srcView);
                bindEntries[1] = BindGroupEntry.Sampler(1U, _sampler);
                BindGroup bindGroup = _device.CreateBindGroup(_bindGroupLayout, bindEntries);

                colorAttachments[0] = new ColorAttachment
                {
                    View = dstView,
                    LoadOp = WGPULoadOp.Clear,
                    StoreOp = WGPUStoreOp.Store,
                    ClearValue = new WGPUColor()
                };

                using (RenderPassEncoder pass = encoder.BeginRenderPass(colorAttachments))
                {
                    pass.SetPipeline(pipeline);
                    pass.SetBindGroup(0U, bindGroup);
                    pass.Draw(3U);
                }

                // Recording the pass took the native refs it needs, so the per-level
                // bind group and views can be released now (before submit).
                bindGroup.Dispose();
                dstView.Dispose();
                srcView.Dispose();
            }
        }
    }

    private unsafe RenderPipeline GetOrCreatePipeline(WGPUTextureFormat format)
    {
        if (_pipelines.TryGetValue(format, out RenderPipeline pipeline))
            return pipeline;

        Span<ColorTargetState> targets = stackalloc ColorTargetState[1];
        targets[0] = new ColorTargetState(format);
        pipeline = _device.CreateRenderPipeline(
            _shaderModule, "vs"u8,
            _shaderModule, "fs"u8,
            targets, _pipelineLayout);
        _pipelines[format] = pipeline;
        return pipeline;
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        foreach (RenderPipeline pipeline in _pipelines.Values)
            pipeline.Dispose();
        _pipelines.Clear();
        _shaderModule.Dispose();
        _pipelineLayout.Dispose();
        _bindGroupLayout.Dispose();
        _sampler.Dispose();
    }
}
