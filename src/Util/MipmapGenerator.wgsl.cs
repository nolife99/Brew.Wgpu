using System;

namespace Brew.Wgpu.Util;

internal static class MipmapGeneratorWgsl
{
    /// <summary>
    /// UTF-8 WGSL for the mip-blit pipeline. The vertex entry <c>vs</c> emits a
    /// full-screen triangle; the fragment entry <c>fs</c> samples the previous
    /// (larger) mip with a linear sampler so writing it into the next (smaller)
    /// mip performs a 2x2 box-style downsample. Stored as a <c>u8</c> literal so
    /// it lives in the data section with no allocation.
    /// </summary>
    public static ReadOnlySpan<byte> Source => """
        struct VertexOutput {
            @builtin(position) position : vec4<f32>,
            @location(0) texCoord : vec2<f32>,
        };

        @vertex
        fn vs(@builtin(vertex_index) vertexIndex : u32) -> VertexOutput {
            var output : VertexOutput;
            let x = f32((vertexIndex << 1u) & 2u);
            let y = f32(vertexIndex & 2u);
            output.texCoord = vec2<f32>(x, y);
            output.position = vec4<f32>(x * 2.0 - 1.0, 1.0 - y * 2.0, 0.0, 1.0);
            return output;
        }

        @group(0) @binding(0) var srcTexture : texture_2d<f32>;
        @group(0) @binding(1) var srcSampler : sampler;

        @fragment
        fn fs(@location(0) texCoord : vec2<f32>) -> @location(0) vec4<f32> {
            return textureSample(srcTexture, srcSampler, texCoord);
        }
        """u8;
}
