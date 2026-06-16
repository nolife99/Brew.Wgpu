using Brew.Wgpu.Native;

#nullable disable
namespace Brew.Wgpu;

public struct BindGroupLayoutEntry
{
    public uint Binding;
    public ShaderStage Visibility;
    public BindGroupLayoutEntry.Kind Type;
    public uint ArraySize;
    public WGPUBufferBindingType BufferType;
    public bool HasDynamicOffset;
    public ulong MinBindingSize;
    public WGPUSamplerBindingType SamplerType;
    public WGPUTextureSampleType TextureSampleType;
    public WGPUTextureViewDimension TextureViewDimension;
    public bool TextureMultisampled;
    public WGPUStorageTextureAccess StorageTextureAccess;
    public WGPUTextureFormat StorageTextureFormat;
    public WGPUTextureViewDimension StorageTextureViewDimension;

    public static BindGroupLayoutEntry Buffer(
      uint binding,
      ShaderStage visibility,
      WGPUBufferBindingType bufferType = WGPUBufferBindingType.Uniform,
      bool hasDynamicOffset = false,
      ulong minBindingSize = 0)
    {
        return new BindGroupLayoutEntry()
        {
            Binding = binding,
            Visibility = visibility,
            Type = BindGroupLayoutEntry.Kind.Buffer,
            BufferType = bufferType,
            HasDynamicOffset = hasDynamicOffset,
            MinBindingSize = minBindingSize
        };
    }

    public static BindGroupLayoutEntry StorageBuffer(
      uint binding,
      ShaderStage visibility,
      bool readOnly = false,
      bool hasDynamicOffset = false,
      ulong minBindingSize = 0)
    {
        return BindGroupLayoutEntry.Buffer(binding, visibility, readOnly ? WGPUBufferBindingType.ReadOnlyStorage : WGPUBufferBindingType.Storage, hasDynamicOffset, minBindingSize);
    }

    public static BindGroupLayoutEntry Sampler(
      uint binding,
      ShaderStage visibility,
      WGPUSamplerBindingType samplerType = WGPUSamplerBindingType.Filtering)
    {
        return new BindGroupLayoutEntry()
        {
            Binding = binding,
            Visibility = visibility,
            Type = BindGroupLayoutEntry.Kind.Sampler,
            SamplerType = samplerType
        };
    }

    public static BindGroupLayoutEntry Texture(
      uint binding,
      ShaderStage visibility,
      WGPUTextureSampleType sampleType = WGPUTextureSampleType.Float,
      WGPUTextureViewDimension viewDimension = WGPUTextureViewDimension._2D,
      bool multisampled = false)
    {
        return new BindGroupLayoutEntry()
        {
            Binding = binding,
            Visibility = visibility,
            Type = BindGroupLayoutEntry.Kind.Texture,
            TextureSampleType = sampleType,
            TextureViewDimension = viewDimension,
            TextureMultisampled = multisampled
        };
    }

    public static BindGroupLayoutEntry StorageTexture(
      uint binding,
      ShaderStage visibility,
      WGPUTextureFormat format,
      WGPUStorageTextureAccess access = WGPUStorageTextureAccess.WriteOnly,
      WGPUTextureViewDimension viewDimension = WGPUTextureViewDimension._2D)
    {
        return new BindGroupLayoutEntry()
        {
            Binding = binding,
            Visibility = visibility,
            Type = BindGroupLayoutEntry.Kind.StorageTexture,
            StorageTextureAccess = access,
            StorageTextureFormat = format,
            StorageTextureViewDimension = viewDimension
        };
    }

    public enum Kind : byte
    {
        Buffer,
        Sampler,
        Texture,
        StorageTexture,
    }
}
