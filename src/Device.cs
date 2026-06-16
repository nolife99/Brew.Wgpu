using Brew.Wgpu.Internal;
using Brew.Wgpu.Native;
using System;
using System.Runtime.InteropServices;

#nullable enable
namespace Brew.Wgpu;

public sealed class Device : IDisposable
{
    private unsafe WGPUDeviceImpl* _handle;
    private readonly Instance _instance;

    public unsafe WGPUDeviceImpl* Handle => this._handle;

    public Queue Queue { get; }

    internal unsafe Device(WGPUDeviceImpl* handle, Instance instance)
    {
        this._handle = handle;
        this._instance = instance;
        this.Queue = new Queue(WGPU.wgpuDeviceGetQueue(handle));
    }

    public void ProcessEvents() => this._instance.ProcessEvents();

    public unsafe bool HasFeature(WGPUFeatureName feature)
    {
        return WGPU.wgpuDeviceHasFeature(this.Handle, feature) > 0U;
    }

    public unsafe WGPUFeatureName[] GetFeatures()
    {
        WGPUSupportedFeatures supportedFeatures = new WGPUSupportedFeatures();
        WGPU.wgpuDeviceGetFeatures(this.Handle, &supportedFeatures);
        WGPUFeatureName[] features = new WGPUFeatureName[(int)supportedFeatures.featureCount];
        for (int index = 0; index < features.Length; ++index)
            features[index] = (WGPUFeatureName)((int*)supportedFeatures.features)[index];
        WGPU.wgpuSupportedFeaturesFreeMembers(supportedFeatures);
        return features;
    }

    public unsafe WGPULimits GetLimits()
    {
        WGPULimits limits = new WGPULimits();
        WGPU.wgpuDeviceGetLimits(this.Handle, &limits);
        return limits;
    }

    public unsafe Sampler CreateSampler(in SamplerDescriptor desc)
    {
        var samplerDesc = new WGPUSamplerDescriptor()
        {
            label = new WGPUStringView()
            {
                data = (sbyte*)null,
                length = WgpuDefaults.StrLen
            },
            addressModeU = desc.AddressModeU,
            addressModeV = desc.AddressModeV,
            addressModeW = desc.AddressModeW,
            magFilter = desc.MagFilter,
            minFilter = desc.MinFilter,
            mipmapFilter = desc.MipmapFilter,
            lodMinClamp = desc.LodMinClamp,
            lodMaxClamp = ((double)desc.LodMaxClamp == 0.0 ? 32f : desc.LodMaxClamp),
            compare = desc.Compare,
            maxAnisotropy = (desc.MaxAnisotropy == (ushort)0 ? (ushort)1 : desc.MaxAnisotropy)
        };
        WGPUSamplerImpl* sampler = WGPU.wgpuDeviceCreateSampler(this.Handle, &samplerDesc);
        return (IntPtr)sampler != IntPtr.Zero ? new Sampler(sampler) : throw new ResourceCreationException("Sampler", "wgpuDeviceCreateSampler");
    }

    public unsafe ShaderModule CreateShaderModule(in ShaderModuleDescriptor desc)
    {
        ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>();
        fixed (byte* numPtr = &desc.Source.Bytes.GetPinnableReference())
        fixed (byte* data = &readOnlySpan.GetPinnableReference())
        {
            WGPUShaderModuleImpl* shaderModule;
            switch (desc.Source.Tag)
            {
                case ShaderSource.Kind.Wgsl:
                    WGPUShaderSourceWGSL shaderSourceWgsl1 = new WGPUShaderSourceWGSL();
                    ref WGPUShaderSourceWGSL local1 = ref shaderSourceWgsl1;
                    WGPUChainedStruct wgpuChainedStruct1 = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)2
                    };
                    local1.chain = wgpuChainedStruct1;
                    ref WGPUShaderSourceWGSL local2 = ref shaderSourceWgsl1;
                    WGPUStringView wgpuStringView = new WGPUStringView()
                    {
                        data = (sbyte*)numPtr,
                        length = (UIntPtr)desc.Source.Bytes.Length
                    };
                    local2.code = wgpuStringView;
                    WGPUShaderSourceWGSL shaderSourceWgsl2 = shaderSourceWgsl1;
                    var __h0 = new WGPUShaderModuleDescriptor()
                    {
                        nextInChain = (WGPUChainedStruct*)&shaderSourceWgsl2,
                        label = WgpuDefaults.OptionalStringView(data, readOnlySpan.Length)
                    };
                    shaderModule = WGPU.wgpuDeviceCreateShaderModule(this.Handle, &__h0);
                    break;
                case ShaderSource.Kind.SpirV:
                    WGPUShaderSourceSPIRV shaderSourceSpirv1 = new WGPUShaderSourceSPIRV();
                    ref WGPUShaderSourceSPIRV local3 = ref shaderSourceSpirv1;
                    WGPUChainedStruct wgpuChainedStruct2 = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)1
                    };
                    local3.chain = wgpuChainedStruct2;
                    shaderSourceSpirv1.codeSize = (uint)(desc.Source.Bytes.Length / 4);
                    shaderSourceSpirv1.code = (uint*)numPtr;
                    WGPUShaderSourceSPIRV shaderSourceSpirv2 = shaderSourceSpirv1;
                    var __h1 = new WGPUShaderModuleDescriptor()
                    {
                        nextInChain = (WGPUChainedStruct*)&shaderSourceSpirv2,
                        label = WgpuDefaults.OptionalStringView(data, readOnlySpan.Length)
                    };
                    shaderModule = WGPU.wgpuDeviceCreateShaderModule(this.Handle, &__h1);
                    break;
                case ShaderSource.Kind.SpirVPassthrough:
                    var __h2 = new WGPUShaderModuleDescriptorSpirV()
                    {
                        label = WgpuDefaults.OptionalStringView(data, readOnlySpan.Length),
                        sourceSize = (uint)(desc.Source.Bytes.Length / 4),
                        source = (uint*)numPtr
                    };
                    WGPUShaderModuleImpl* shaderModuleSpirV = WGPU.wgpuDeviceCreateShaderModuleSpirV(this.Handle, &__h2);
                    return (IntPtr)shaderModuleSpirV != IntPtr.Zero ? new ShaderModule(shaderModuleSpirV) : throw new ResourceCreationException("ShaderModule", "wgpuDeviceCreateShaderModuleSpirV");
                default:
                    throw new ArgumentOutOfRangeException(nameof(desc), $"Unknown ShaderSource.Kind: {desc.Source.Tag}");
            }
            return (IntPtr)shaderModule != IntPtr.Zero ? new ShaderModule(shaderModule) : throw new ResourceCreationException("ShaderModule", "wgpuDeviceCreateShaderModule");
        }
    }

    public unsafe Buffer CreateBuffer(in BufferDescriptor desc)
    {
        ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>();
        fixed (byte* data = &readOnlySpan.GetPinnableReference())
        {
            var __h3 = new WGPUBufferDescriptor()
            {
                label = WgpuDefaults.OptionalStringView(data, readOnlySpan.Length),
                usage = (ulong)desc.Usage,
                size = desc.Size,
                mappedAtCreation = desc.MappedAtCreation ? 1u : 0u
            };
            WGPUBufferImpl* buffer = WGPU.wgpuDeviceCreateBuffer(this.Handle, &__h3);
            return (IntPtr)buffer != IntPtr.Zero ? new Buffer(buffer) : throw new ResourceCreationException("Buffer", "wgpuDeviceCreateBuffer");
        }
    }

    public unsafe Buffer CreateBufferInit<T>(
      in BufferInitDescriptor desc,
      scoped ReadOnlySpan<T> data)
      where T : unmanaged
    {
        if (data.IsEmpty)
            throw new ArgumentException("CreateBufferInit requires non-empty data. Use CreateBuffer for a zero-sized buffer.", nameof(data));
        ulong length = (ulong)data.Length * (ulong)sizeof(T);
        ulong num = length + 3UL & 18446744073709551612UL;
        ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>();
        fixed (byte* data1 = &readOnlySpan.GetPinnableReference())
        {
            var __h4 = new WGPUBufferDescriptor()
            {
                label = WgpuDefaults.OptionalStringView(data1, readOnlySpan.Length),
                usage = (ulong)desc.Usage,
                size = num,
                mappedAtCreation = 1U
            };
            WGPUBufferImpl* buffer = WGPU.wgpuDeviceCreateBuffer(this.Handle, &__h4);
            void* pointer = (IntPtr)buffer != IntPtr.Zero ? WGPU.wgpuBufferGetMappedRange(buffer, UIntPtr.Zero, (UIntPtr)num) : throw new ResourceCreationException("Buffer", "wgpuDeviceCreateBuffer");
            if ((IntPtr)pointer == IntPtr.Zero)
            {
                WGPU.wgpuBufferRelease(buffer);
                throw new InvalidOperationException("wgpuBufferGetMappedRange returned null for a fresh mappedAtCreation buffer.");
            }
            try
            {
                MemoryMarshal.AsBytes<T>(data).CopyTo(new Span<byte>(pointer, (int)length));
                WGPU.wgpuBufferUnmap(buffer);
            }
            catch
            {
                WGPU.wgpuBufferRelease(buffer);
                throw;
            }
            return new Buffer(buffer);
        }
    }

    public unsafe Texture CreateTexture(in TextureDescriptor desc)
    {
        ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>();
        fixed (byte* data = &readOnlySpan.GetPinnableReference())
        fixed (WGPUTextureFormat* wgpuTextureFormatPtr = &MemoryMarshal.GetReference<WGPUTextureFormat>(desc.ViewFormats))
        {
            var __h5 = new WGPUTextureDescriptor()
            {
                label = WgpuDefaults.OptionalStringView(data, readOnlySpan.Length),
                usage = (ulong)desc.Usage,
                dimension = desc.Dimension,
                size = desc.Size,
                format = desc.Format,
                mipLevelCount = (desc.MipLevelCount == 0U ? 1U : desc.MipLevelCount),
                sampleCount = (desc.SampleCount == 0U ? 1U : desc.SampleCount),
                viewFormatCount = (UIntPtr)desc.ViewFormats.Length,
                viewFormats = (desc.ViewFormats.IsEmpty ? (WGPUTextureFormat*)null : wgpuTextureFormatPtr)
            };
            WGPUTextureImpl* texture = WGPU.wgpuDeviceCreateTexture(this.Handle, &__h5);
            return (IntPtr)texture != IntPtr.Zero ? new Texture(texture) : throw new ResourceCreationException("Texture", "wgpuDeviceCreateTexture");
        }
    }

    public unsafe BindGroupLayout CreateBindGroupLayout(
      scoped ReadOnlySpan<BindGroupLayoutEntry> entries)
    {
        int length = entries.Length;
        Span<WGPUBindGroupLayoutEntry> span1 = stackalloc WGPUBindGroupLayoutEntry[length];
        Span<WGPUBindGroupLayoutEntryExtras> span2 = stackalloc WGPUBindGroupLayoutEntryExtras[length];
        for (int index = 0; index < length; ++index)
        {
            ref readonly BindGroupLayoutEntry local = ref entries[index];
            WGPUBindGroupLayoutEntry groupLayoutEntry = new WGPUBindGroupLayoutEntry()
            {
                binding = local.Binding,
                visibility = (ulong)local.Visibility
            };
            switch (local.Type)
            {
                case BindGroupLayoutEntry.Kind.Buffer:
                    groupLayoutEntry.buffer = new WGPUBufferBindingLayout()
                    {
                        type = local.BufferType,
                        hasDynamicOffset = local.HasDynamicOffset ? 1u : 0u,
                        minBindingSize = local.MinBindingSize
                    };
                    break;
                case BindGroupLayoutEntry.Kind.Sampler:
                    groupLayoutEntry.sampler = new WGPUSamplerBindingLayout()
                    {
                        type = local.SamplerType
                    };
                    break;
                case BindGroupLayoutEntry.Kind.Texture:
                    groupLayoutEntry.texture = new WGPUTextureBindingLayout()
                    {
                        sampleType = local.TextureSampleType,
                        viewDimension = local.TextureViewDimension,
                        multisampled = local.TextureMultisampled ? 1u : 0u
                    };
                    break;
                case BindGroupLayoutEntry.Kind.StorageTexture:
                    groupLayoutEntry.storageTexture = new WGPUStorageTextureBindingLayout()
                    {
                        access = local.StorageTextureAccess,
                        format = local.StorageTextureFormat,
                        viewDimension = local.StorageTextureViewDimension
                    };
                    break;
            }
            if (local.ArraySize > 0U)
                span2[index] = new WGPUBindGroupLayoutEntryExtras()
                {
                    chain = new WGPUChainedStruct()
                    {
                        sType = (WGPUSType)196616 /*0x030008*/
                    },
                    count = local.ArraySize
                };
            span1[index] = groupLayoutEntry;
        }
        fixed (WGPUBindGroupLayoutEntry* groupLayoutEntryPtr = &span1.GetPinnableReference())
        fixed (WGPUBindGroupLayoutEntryExtras* layoutEntryExtrasPtr = &span2.GetPinnableReference())
        {
            for (int index = 0; index < length; ++index)
            {
                if (entries[index].ArraySize > 0U)
                    groupLayoutEntryPtr[index].nextInChain = (WGPUChainedStruct*)(layoutEntryExtrasPtr + index);
            }
            var __h6 = new WGPUBindGroupLayoutDescriptor()
            {
                entryCount = (UIntPtr)length,
                entries = groupLayoutEntryPtr
            };
            WGPUBindGroupLayoutImpl* bindGroupLayout = WGPU.wgpuDeviceCreateBindGroupLayout(this.Handle, &__h6);
            return (IntPtr)bindGroupLayout != IntPtr.Zero ? new BindGroupLayout(bindGroupLayout) : throw new ResourceCreationException("BindGroupLayout", "wgpuDeviceCreateBindGroupLayout");
        }
    }

    public PipelineLayout CreatePipelineLayout(
      scoped ReadOnlySpan<BindGroupLayout> bindGroupLayouts)
    {
        return this.CreatePipelineLayout(bindGroupLayouts, 0U);
    }

    public unsafe PipelineLayout CreatePipelineLayout(
      scoped ReadOnlySpan<BindGroupLayout> bindGroupLayouts,
      uint pushConstantBytes)
    {
        if (pushConstantBytes > 0U)
            throw new NotSupportedException(
              "Push-constant pipeline layouts are not supported by this build of Brew.Wgpu.");
        ReadOnlySpan<IntPtr> readOnlySpan = MemoryMarshal.Cast<BindGroupLayout, IntPtr>(bindGroupLayouts);
        fixed (IntPtr* numPtr = &readOnlySpan.GetPinnableReference())
        {
            var __h7 = new WGPUPipelineLayoutDescriptor()
            {
                bindGroupLayoutCount = (UIntPtr)readOnlySpan.Length,
                bindGroupLayouts = (WGPUBindGroupLayoutImpl**)numPtr
            };
            WGPUPipelineLayoutImpl* pipelineLayout = WGPU.wgpuDeviceCreatePipelineLayout(this.Handle, &__h7);
            return (IntPtr)pipelineLayout != IntPtr.Zero ? new PipelineLayout(pipelineLayout) : throw new ResourceCreationException("PipelineLayout", "wgpuDeviceCreatePipelineLayout");
        }
    }

    public BindGroup CreateBindGroup(
      BindGroupLayout layout,
      scoped ReadOnlySpan<BindGroupEntry> entries)
    {
        return this.CreateBindGroupBindless(layout, entries, uint.MaxValue, new ReadOnlySpan<TextureView>());
    }

    public unsafe BindGroup CreateBindGroupBindless(
      BindGroupLayout layout,
      scoped ReadOnlySpan<BindGroupEntry> entries,
      uint arrayBinding,
      scoped ReadOnlySpan<TextureView> arrayTextureViews)
    {
        int length1 = entries.Length;
        int length2 = arrayTextureViews.Length;
        bool flag = arrayBinding != uint.MaxValue && length2 > 0;
        int num = length1 + (flag ? 1 : 0);
        Span<WGPUBindGroupEntry> span = stackalloc WGPUBindGroupEntry[num];
        for (int index = 0; index < length1; ++index)
        {
            ref readonly BindGroupEntry local = ref entries[index];
            WGPUBindGroupEntry wgpuBindGroupEntry = new WGPUBindGroupEntry()
            {
                binding = local.Binding
            };
            switch (local.Type)
            {
                case BindGroupEntry.Kind.Buffer:
                    wgpuBindGroupEntry.buffer = local.BufferResource.Handle;
                    wgpuBindGroupEntry.offset = local.BufferOffset;
                    wgpuBindGroupEntry.size = local.BufferSize;
                    break;
                case BindGroupEntry.Kind.Sampler:
                    wgpuBindGroupEntry.sampler = local.SamplerResource.Handle;
                    break;
                case BindGroupEntry.Kind.TextureView:
                    wgpuBindGroupEntry.textureView = local.TextureViewResource.Handle;
                    break;
            }
            span[index] = wgpuBindGroupEntry;
        }
        ReadOnlySpan<IntPtr> readOnlySpan = MemoryMarshal.Cast<TextureView, IntPtr>(arrayTextureViews);
        fixed (WGPUBindGroupEntry* wgpuBindGroupEntryPtr = &span.GetPinnableReference())
        fixed (IntPtr* numPtr = &readOnlySpan.GetPinnableReference())
        {
            WGPUBindGroupEntryExtras groupEntryExtras1 = new WGPUBindGroupEntryExtras();
            if (flag)
            {
                WGPUBindGroupEntryExtras groupEntryExtras2 = new WGPUBindGroupEntryExtras();
                ref WGPUBindGroupEntryExtras local1 = ref groupEntryExtras2;
                WGPUChainedStruct wgpuChainedStruct = new WGPUChainedStruct()
                {
                    sType = (WGPUSType)196615 /*0x030007*/
                };
                local1.chain = wgpuChainedStruct;
                groupEntryExtras2.textureViews = (WGPUTextureViewImpl**)numPtr;
                groupEntryExtras2.textureViewCount = (UIntPtr)length2;
                WGPUBindGroupEntryExtras groupEntryExtras3 = groupEntryExtras2;
                ref WGPUBindGroupEntry local2 = ref span[length1];
                WGPUBindGroupEntry wgpuBindGroupEntry = new WGPUBindGroupEntry()
                {
                    binding = arrayBinding,
                    nextInChain = (WGPUChainedStruct*)&groupEntryExtras3
                };
                local2 = wgpuBindGroupEntry;
            }
            var __h8 = new WGPUBindGroupDescriptor()
            {
                layout = layout.Handle,
                entryCount = (UIntPtr)num,
                entries = wgpuBindGroupEntryPtr
            };
            WGPUBindGroupImpl* bindGroup = WGPU.wgpuDeviceCreateBindGroup(this.Handle, &__h8);
            return (IntPtr)bindGroup != IntPtr.Zero ? new BindGroup(bindGroup) : throw new ResourceCreationException("BindGroup", "wgpuDeviceCreateBindGroup");
        }
    }

    public unsafe RenderPipeline CreateRenderPipeline(
      ShaderModule vertexShader,
      scoped ReadOnlySpan<byte> vertexEntryPoint,
      ShaderModule fragmentShader,
      scoped ReadOnlySpan<byte> fragmentEntryPoint,
      scoped ReadOnlySpan<ColorTargetState> fragmentTargets,
      PipelineLayout layout = default(PipelineLayout),
      PrimitiveState primitive = default(PrimitiveState),
      MultisampleState multisample = default(MultisampleState),
      in DepthStencilState depthStencil = default(DepthStencilState),
      ulong vertexStride = 0,
      WGPUVertexStepMode vertexStepMode = (WGPUVertexStepMode)0,
      scoped ReadOnlySpan<VertexAttribute> vertexAttributes = default(ReadOnlySpan<VertexAttribute>))
    {
        if (primitive.Topology == null)
            primitive = PrimitiveState.Default;
        if (multisample.Count == 0U)
            multisample = MultisampleState.Default;
        int length1 = fragmentTargets.Length;
        Span<WGPUColorTargetState> span1 = stackalloc WGPUColorTargetState[length1];
        Span<WGPUBlendState> span2 = stackalloc WGPUBlendState[length1];
        for (int index = 0; index < length1; ++index)
        {
            ref readonly ColorTargetState local = ref fragmentTargets[index];
            ColorWriteMask colorWriteMask = local.WriteMask == ColorWriteMask.None ? ColorWriteMask.All : local.WriteMask;
            WGPUBlendState* wgpuBlendStatePtr1 = (WGPUBlendState*)null;
            WGPUBlendState? blend = local.Blend;
            if (blend.HasValue)
            {
                WGPUBlendState valueOrDefault = blend.GetValueOrDefault();
                span2[index] = valueOrDefault;
                fixed (WGPUBlendState* wgpuBlendStatePtr2 = &span2[index])
                    wgpuBlendStatePtr1 = wgpuBlendStatePtr2;
            }
            span1[index] = new WGPUColorTargetState()
            {
                format = local.Format,
                blend = wgpuBlendStatePtr1,
                writeMask = (ulong)colorWriteMask
            };
        }
        int length2 = vertexAttributes.Length;
        Span<WGPUVertexAttribute> span3 = length2 <= 0 ? new Span<WGPUVertexAttribute>() : stackalloc WGPUVertexAttribute[length2];
        for (int index = 0; index < length2; ++index)
        {
            ref readonly VertexAttribute local = ref vertexAttributes[index];
            span3[index] = new WGPUVertexAttribute()
            {
                format = local.Format,
                offset = local.Offset,
                shaderLocation = local.ShaderLocation
            };
        }
        bool flag = depthStencil.Format > 0;
        WGPUDepthStencilState depthStencilState = new WGPUDepthStencilState();
        if (flag)
        {
            WGPUStencilFaceState stencilFaceState = new WGPUStencilFaceState()
            {
                compare = (WGPUCompareFunction)0,
                failOp = (WGPUStencilOperation)0,
                depthFailOp = (WGPUStencilOperation)0,
                passOp = (WGPUStencilOperation)0
            };
            depthStencilState = new WGPUDepthStencilState()
            {
                format = depthStencil.Format,
                depthWriteEnabled = depthStencil.DepthWriteEnabled ? (WGPUOptionalBool)1 : (WGPUOptionalBool)0,
                depthCompare = depthStencil.DepthCompare,
                stencilFront = stencilFaceState,
                stencilBack = stencilFaceState,
                stencilReadMask = depthStencil.StencilReadMask == 0U ? uint.MaxValue : depthStencil.StencilReadMask,
                stencilWriteMask = depthStencil.StencilWriteMask == 0U ? uint.MaxValue : depthStencil.StencilWriteMask,
                depthBias = depthStencil.DepthBias,
                depthBiasSlopeScale = depthStencil.DepthBiasSlopeScale,
                depthBiasClamp = depthStencil.DepthBiasClamp
            };
        }
        fixed (byte* numPtr1 = &vertexEntryPoint.GetPinnableReference())
        fixed (byte* numPtr2 = &fragmentEntryPoint.GetPinnableReference())
        fixed (WGPUColorTargetState* colorTargetStatePtr = &span1.GetPinnableReference())
        fixed (WGPUVertexAttribute* wgpuVertexAttributePtr = &span3.GetPinnableReference())
        {
            WGPUVertexBufferLayout vertexBufferLayout = new WGPUVertexBufferLayout()
            {
                stepMode = vertexStepMode == null ? (WGPUVertexStepMode)(object)1 : vertexStepMode,
                arrayStride = vertexStride,
                attributeCount = (UIntPtr)length2,
                attributes = wgpuVertexAttributePtr
            };
            WGPUFragmentState wgpuFragmentState1 = new WGPUFragmentState();
            wgpuFragmentState1.module = fragmentShader.Handle;
            ref WGPUFragmentState local1 = ref wgpuFragmentState1;
            WGPUStringView wgpuStringView1 = new WGPUStringView();
            wgpuStringView1.data = (sbyte*)numPtr2;
            wgpuStringView1.length = (UIntPtr)fragmentEntryPoint.Length;
            WGPUStringView wgpuStringView2 = wgpuStringView1;
            local1.entryPoint = wgpuStringView2;
            wgpuFragmentState1.targetCount = (UIntPtr)length1;
            wgpuFragmentState1.targets = colorTargetStatePtr;
            WGPUFragmentState wgpuFragmentState2 = wgpuFragmentState1;
            WGPURenderPipelineDescriptor pipelineDescriptor = new WGPURenderPipelineDescriptor();
            pipelineDescriptor.layout = layout.Handle;
            ref WGPURenderPipelineDescriptor local2 = ref pipelineDescriptor;
            WGPUVertexState wgpuVertexState1 = new WGPUVertexState();
            wgpuVertexState1.module = vertexShader.Handle;
            ref WGPUVertexState local3 = ref wgpuVertexState1;
            wgpuStringView1 = new WGPUStringView();
            wgpuStringView1.data = (sbyte*)numPtr1;
            wgpuStringView1.length = (UIntPtr)vertexEntryPoint.Length;
            WGPUStringView wgpuStringView3 = wgpuStringView1;
            local3.entryPoint = wgpuStringView3;
            wgpuVertexState1.bufferCount = (UIntPtr)(length2 > 0 ? 1 : 0);
            wgpuVertexState1.buffers = length2 > 0 ? &vertexBufferLayout : (WGPUVertexBufferLayout*)null;
            WGPUVertexState wgpuVertexState2 = wgpuVertexState1;
            local2.vertex = wgpuVertexState2;
            ref WGPURenderPipelineDescriptor local4 = ref pipelineDescriptor;
            WGPUPrimitiveState wgpuPrimitiveState = new WGPUPrimitiveState()
            {
                topology = primitive.Topology,
                stripIndexFormat = primitive.StripIndexFormat,
                frontFace = primitive.FrontFace,
                cullMode = primitive.CullMode,
                unclippedDepth = primitive.UnclippedDepth ? 1u : 0u
            };
            local4.primitive = wgpuPrimitiveState;
            ref WGPURenderPipelineDescriptor local5 = ref pipelineDescriptor;
            WGPUMultisampleState multisampleState = new WGPUMultisampleState()
            {
                count = multisample.Count,
                mask = multisample.Mask,
                alphaToCoverageEnabled = multisample.AlphaToCoverageEnabled ? 1u : 0u
            };
            local5.multisample = multisampleState;
            pipelineDescriptor.depthStencil = flag ? &depthStencilState : (WGPUDepthStencilState*)null;
            pipelineDescriptor.fragment = &wgpuFragmentState2;
            WGPURenderPipelineImpl* renderPipeline = WGPU.wgpuDeviceCreateRenderPipeline(this.Handle, &pipelineDescriptor);
            return (IntPtr)renderPipeline != IntPtr.Zero ? new RenderPipeline(renderPipeline) : throw new ResourceCreationException("RenderPipeline", "wgpuDeviceCreateRenderPipeline");
        }
    }

    public unsafe RenderPipeline CreateRenderPipeline(
      ShaderModule vertexShader,
      scoped ReadOnlySpan<byte> vertexEntryPoint,
      ShaderModule fragmentShader,
      scoped ReadOnlySpan<byte> fragmentEntryPoint,
      scoped ReadOnlySpan<ColorTargetState> fragmentTargets,
      scoped ReadOnlySpan<VertexBufferLayout> vertexBuffers,
      scoped ReadOnlySpan<VertexAttribute> vertexAttributes,
      PipelineLayout layout = default(PipelineLayout),
      PrimitiveState primitive = default(PrimitiveState),
      MultisampleState multisample = default(MultisampleState),
      in DepthStencilState depthStencil = default(DepthStencilState))
    {
        if (primitive.Topology == null)
            primitive = PrimitiveState.Default;
        if (multisample.Count == 0U)
            multisample = MultisampleState.Default;
        int length1 = vertexBuffers.Length;
        int num1 = 0;
        for (int index = 0; index < length1; ++index)
            num1 += vertexBuffers[index].AttributeCount;
        if (num1 > vertexAttributes.Length)
            throw new ArgumentException($"vertexBuffers requests {num1} attributes total but vertexAttributes span only has {vertexAttributes.Length} elements.", nameof(vertexAttributes));
        int length2 = fragmentTargets.Length;
        Span<WGPUColorTargetState> span1 = stackalloc WGPUColorTargetState[length2];
        Span<WGPUBlendState> span2 = stackalloc WGPUBlendState[length2];
        for (int index = 0; index < length2; ++index)
        {
            ref readonly ColorTargetState local = ref fragmentTargets[index];
            ColorWriteMask colorWriteMask = local.WriteMask == ColorWriteMask.None ? ColorWriteMask.All : local.WriteMask;
            WGPUBlendState* wgpuBlendStatePtr1 = (WGPUBlendState*)null;
            WGPUBlendState? blend = local.Blend;
            if (blend.HasValue)
            {
                WGPUBlendState valueOrDefault = blend.GetValueOrDefault();
                span2[index] = valueOrDefault;
                fixed (WGPUBlendState* wgpuBlendStatePtr2 = &span2[index])
                    wgpuBlendStatePtr1 = wgpuBlendStatePtr2;
            }
            span1[index] = new WGPUColorTargetState()
            {
                format = local.Format,
                blend = wgpuBlendStatePtr1,
                writeMask = (ulong)colorWriteMask
            };
        }
        Span<WGPUVertexAttribute> span3 = num1 <= 0 ? new Span<WGPUVertexAttribute>() : stackalloc WGPUVertexAttribute[num1];
        Span<WGPUVertexBufferLayout> span4 = length1 <= 0 ? new Span<WGPUVertexBufferLayout>() : stackalloc WGPUVertexBufferLayout[length1];
        bool flag = depthStencil.Format > 0;
        WGPUDepthStencilState depthStencilState = new WGPUDepthStencilState();
        if (flag)
        {
            WGPUStencilFaceState stencilFaceState = new WGPUStencilFaceState()
            {
                compare = (WGPUCompareFunction)0,
                failOp = (WGPUStencilOperation)0,
                depthFailOp = (WGPUStencilOperation)0,
                passOp = (WGPUStencilOperation)0
            };
            depthStencilState = new WGPUDepthStencilState()
            {
                format = depthStencil.Format,
                depthWriteEnabled = depthStencil.DepthWriteEnabled ? (WGPUOptionalBool)1 : (WGPUOptionalBool)0,
                depthCompare = depthStencil.DepthCompare,
                stencilFront = stencilFaceState,
                stencilBack = stencilFaceState,
                stencilReadMask = depthStencil.StencilReadMask == 0U ? uint.MaxValue : depthStencil.StencilReadMask,
                stencilWriteMask = depthStencil.StencilWriteMask == 0U ? uint.MaxValue : depthStencil.StencilWriteMask,
                depthBias = depthStencil.DepthBias,
                depthBiasSlopeScale = depthStencil.DepthBiasSlopeScale,
                depthBiasClamp = depthStencil.DepthBiasClamp
            };
        }
        fixed (byte* numPtr1 = &vertexEntryPoint.GetPinnableReference())
        fixed (byte* numPtr2 = &fragmentEntryPoint.GetPinnableReference())
        fixed (WGPUColorTargetState* colorTargetStatePtr = &span1.GetPinnableReference())
        fixed (WGPUVertexAttribute* wgpuVertexAttributePtr1 = &span3.GetPinnableReference())
        fixed (WGPUVertexBufferLayout* vertexBufferLayoutPtr1 = &span4.GetPinnableReference())
        {
            int num2 = 0;
            for (int index1 = 0; index1 < length1; ++index1)
            {
                ref readonly VertexBufferLayout local1 = ref vertexBuffers[index1];
                int attributeCount = local1.AttributeCount;
                for (int index2 = 0; index2 < attributeCount; ++index2)
                {
                    ref readonly VertexAttribute local2 = ref vertexAttributes[num2 + index2];
                    WGPUVertexAttribute* wgpuVertexAttributePtr2 = wgpuVertexAttributePtr1 + (num2 + index2);
                    WGPUVertexAttribute wgpuVertexAttribute = new WGPUVertexAttribute()
                    {
                        format = local2.Format,
                        offset = local2.Offset,
                        shaderLocation = local2.ShaderLocation
                    };
                    *wgpuVertexAttributePtr2 = wgpuVertexAttribute;
                }
                WGPUVertexBufferLayout* vertexBufferLayoutPtr2 = vertexBufferLayoutPtr1 + index1;
                WGPUVertexBufferLayout vertexBufferLayout = new WGPUVertexBufferLayout()
                {
                    stepMode = local1.StepMode == null ? (WGPUVertexStepMode)(object)1 : local1.StepMode,
                    arrayStride = local1.ArrayStride,
                    attributeCount = (UIntPtr)attributeCount,
                    attributes = attributeCount > 0 ? wgpuVertexAttributePtr1 + num2 : (WGPUVertexAttribute*)null
                };
                *vertexBufferLayoutPtr2 = vertexBufferLayout;
                num2 += attributeCount;
            }
            WGPUFragmentState wgpuFragmentState1 = new WGPUFragmentState();
            wgpuFragmentState1.module = fragmentShader.Handle;
            ref WGPUFragmentState local3 = ref wgpuFragmentState1;
            WGPUStringView wgpuStringView1 = new WGPUStringView();
            wgpuStringView1.data = (sbyte*)numPtr2;
            wgpuStringView1.length = (UIntPtr)fragmentEntryPoint.Length;
            WGPUStringView wgpuStringView2 = wgpuStringView1;
            local3.entryPoint = wgpuStringView2;
            wgpuFragmentState1.targetCount = (UIntPtr)length2;
            wgpuFragmentState1.targets = colorTargetStatePtr;
            WGPUFragmentState wgpuFragmentState2 = wgpuFragmentState1;
            WGPURenderPipelineDescriptor pipelineDescriptor = new WGPURenderPipelineDescriptor();
            pipelineDescriptor.layout = layout.Handle;
            ref WGPURenderPipelineDescriptor local4 = ref pipelineDescriptor;
            WGPUVertexState wgpuVertexState1 = new WGPUVertexState();
            wgpuVertexState1.module = vertexShader.Handle;
            ref WGPUVertexState local5 = ref wgpuVertexState1;
            wgpuStringView1 = new WGPUStringView();
            wgpuStringView1.data = (sbyte*)numPtr1;
            wgpuStringView1.length = (UIntPtr)vertexEntryPoint.Length;
            WGPUStringView wgpuStringView3 = wgpuStringView1;
            local5.entryPoint = wgpuStringView3;
            wgpuVertexState1.bufferCount = (UIntPtr)length1;
            wgpuVertexState1.buffers = length1 > 0 ? vertexBufferLayoutPtr1 : (WGPUVertexBufferLayout*)null;
            WGPUVertexState wgpuVertexState2 = wgpuVertexState1;
            local4.vertex = wgpuVertexState2;
            ref WGPURenderPipelineDescriptor local6 = ref pipelineDescriptor;
            WGPUPrimitiveState wgpuPrimitiveState = new WGPUPrimitiveState()
            {
                topology = primitive.Topology,
                stripIndexFormat = primitive.StripIndexFormat,
                frontFace = primitive.FrontFace,
                cullMode = primitive.CullMode,
                unclippedDepth = primitive.UnclippedDepth ? 1u : 0u
            };
            local6.primitive = wgpuPrimitiveState;
            ref WGPURenderPipelineDescriptor local7 = ref pipelineDescriptor;
            WGPUMultisampleState multisampleState = new WGPUMultisampleState()
            {
                count = multisample.Count,
                mask = multisample.Mask,
                alphaToCoverageEnabled = multisample.AlphaToCoverageEnabled ? 1u : 0u
            };
            local7.multisample = multisampleState;
            pipelineDescriptor.depthStencil = flag ? &depthStencilState : (WGPUDepthStencilState*)null;
            pipelineDescriptor.fragment = &wgpuFragmentState2;
            WGPURenderPipelineImpl* renderPipeline = WGPU.wgpuDeviceCreateRenderPipeline(this.Handle, &pipelineDescriptor);
            return (IntPtr)renderPipeline != IntPtr.Zero ? new RenderPipeline(renderPipeline) : throw new ResourceCreationException("RenderPipeline", "wgpuDeviceCreateRenderPipeline");
        }
    }

    public unsafe RenderPipeline CreateRenderPipeline(
      ShaderModule vertexShader,
      scoped ReadOnlySpan<byte> vertexEntryPoint,
      PipelineLayout layout = default(PipelineLayout),
      PrimitiveState primitive = default(PrimitiveState),
      MultisampleState multisample = default(MultisampleState),
      in DepthStencilState depthStencil = default(DepthStencilState),
      ulong vertexStride = 0,
      WGPUVertexStepMode vertexStepMode = (WGPUVertexStepMode)0,
      scoped ReadOnlySpan<VertexAttribute> vertexAttributes = default(ReadOnlySpan<VertexAttribute>))
    {
        if (primitive.Topology == null)
            primitive = PrimitiveState.Default;
        if (multisample.Count == 0U)
            multisample = MultisampleState.Default;
        int length = vertexAttributes.Length;
        Span<WGPUVertexAttribute> span = length <= 0 ? new Span<WGPUVertexAttribute>() : stackalloc WGPUVertexAttribute[length];
        for (int index = 0; index < length; ++index)
        {
            ref readonly VertexAttribute local = ref vertexAttributes[index];
            span[index] = new WGPUVertexAttribute()
            {
                format = local.Format,
                offset = local.Offset,
                shaderLocation = local.ShaderLocation
            };
        }
        bool flag = depthStencil.Format > 0;
        WGPUDepthStencilState depthStencilState = new WGPUDepthStencilState();
        if (flag)
            depthStencilState = Device.BuildDepthStencil(in depthStencil);
        fixed (byte* numPtr = &vertexEntryPoint.GetPinnableReference())
        fixed (WGPUVertexAttribute* wgpuVertexAttributePtr = &span.GetPinnableReference())
        {
            WGPUVertexBufferLayout vertexBufferLayout = new WGPUVertexBufferLayout()
            {
                stepMode = vertexStepMode == null ? (WGPUVertexStepMode)(object)1 : vertexStepMode,
                arrayStride = vertexStride,
                attributeCount = (UIntPtr)length,
                attributes = wgpuVertexAttributePtr
            };
            WGPURenderPipelineDescriptor pipelineDescriptor = new WGPURenderPipelineDescriptor();
            pipelineDescriptor.layout = layout.Handle;
            ref WGPURenderPipelineDescriptor local1 = ref pipelineDescriptor;
            WGPUVertexState wgpuVertexState1 = new WGPUVertexState();
            wgpuVertexState1.module = vertexShader.Handle;
            ref WGPUVertexState local2 = ref wgpuVertexState1;
            WGPUStringView wgpuStringView = new WGPUStringView()
            {
                data = (sbyte*)numPtr,
                length = (UIntPtr)vertexEntryPoint.Length
            };
            local2.entryPoint = wgpuStringView;
            wgpuVertexState1.bufferCount = (UIntPtr)(length > 0 ? 1 : 0);
            wgpuVertexState1.buffers = length > 0 ? &vertexBufferLayout : (WGPUVertexBufferLayout*)null;
            WGPUVertexState wgpuVertexState2 = wgpuVertexState1;
            local1.vertex = wgpuVertexState2;
            pipelineDescriptor.primitive = Device.BuildPrimitive(primitive);
            pipelineDescriptor.multisample = Device.BuildMultisample(multisample);
            pipelineDescriptor.depthStencil = flag ? &depthStencilState : (WGPUDepthStencilState*)null;
            pipelineDescriptor.fragment = (WGPUFragmentState*)null;
            WGPURenderPipelineImpl* renderPipeline = WGPU.wgpuDeviceCreateRenderPipeline(this.Handle, &pipelineDescriptor);
            return (IntPtr)renderPipeline != IntPtr.Zero ? new RenderPipeline(renderPipeline) : throw new ResourceCreationException("RenderPipeline", "wgpuDeviceCreateRenderPipeline");
        }
    }

    public unsafe RenderPipeline CreateRenderPipeline(
      ShaderModule vertexShader,
      scoped ReadOnlySpan<byte> vertexEntryPoint,
      scoped ReadOnlySpan<VertexBufferLayout> vertexBuffers,
      scoped ReadOnlySpan<VertexAttribute> vertexAttributes,
      PipelineLayout layout = default(PipelineLayout),
      PrimitiveState primitive = default(PrimitiveState),
      MultisampleState multisample = default(MultisampleState),
      in DepthStencilState depthStencil = default(DepthStencilState))
    {
        if (primitive.Topology == null)
            primitive = PrimitiveState.Default;
        if (multisample.Count == 0U)
            multisample = MultisampleState.Default;
        int length = vertexBuffers.Length;
        int num1 = 0;
        for (int index = 0; index < length; ++index)
            num1 += vertexBuffers[index].AttributeCount;
        if (num1 > vertexAttributes.Length)
            throw new ArgumentException($"vertexBuffers requests {num1} attributes total but vertexAttributes span only has {vertexAttributes.Length} elements.", nameof(vertexAttributes));
        Span<WGPUVertexAttribute> span1 = num1 <= 0 ? new Span<WGPUVertexAttribute>() : stackalloc WGPUVertexAttribute[num1];
        Span<WGPUVertexBufferLayout> span2 = length <= 0 ? new Span<WGPUVertexBufferLayout>() : stackalloc WGPUVertexBufferLayout[length];
        bool flag = depthStencil.Format > 0;
        WGPUDepthStencilState depthStencilState = new WGPUDepthStencilState();
        if (flag)
            depthStencilState = Device.BuildDepthStencil(in depthStencil);
        fixed (byte* numPtr = &vertexEntryPoint.GetPinnableReference())
        fixed (WGPUVertexAttribute* wgpuVertexAttributePtr1 = &span1.GetPinnableReference())
        fixed (WGPUVertexBufferLayout* vertexBufferLayoutPtr1 = &span2.GetPinnableReference())
        {
            int num2 = 0;
            for (int index1 = 0; index1 < length; ++index1)
            {
                ref readonly VertexBufferLayout local1 = ref vertexBuffers[index1];
                int attributeCount = local1.AttributeCount;
                for (int index2 = 0; index2 < attributeCount; ++index2)
                {
                    ref readonly VertexAttribute local2 = ref vertexAttributes[num2 + index2];
                    WGPUVertexAttribute* wgpuVertexAttributePtr2 = wgpuVertexAttributePtr1 + (num2 + index2);
                    WGPUVertexAttribute wgpuVertexAttribute = new WGPUVertexAttribute()
                    {
                        format = local2.Format,
                        offset = local2.Offset,
                        shaderLocation = local2.ShaderLocation
                    };
                    *wgpuVertexAttributePtr2 = wgpuVertexAttribute;
                }
                WGPUVertexBufferLayout* vertexBufferLayoutPtr2 = vertexBufferLayoutPtr1 + index1;
                WGPUVertexBufferLayout vertexBufferLayout = new WGPUVertexBufferLayout()
                {
                    stepMode = local1.StepMode == null ? (WGPUVertexStepMode)(object)1 : local1.StepMode,
                    arrayStride = local1.ArrayStride,
                    attributeCount = (UIntPtr)attributeCount,
                    attributes = attributeCount > 0 ? wgpuVertexAttributePtr1 + num2 : (WGPUVertexAttribute*)null
                };
                *vertexBufferLayoutPtr2 = vertexBufferLayout;
                num2 += attributeCount;
            }
            WGPURenderPipelineDescriptor pipelineDescriptor = new WGPURenderPipelineDescriptor();
            pipelineDescriptor.layout = layout.Handle;
            ref WGPURenderPipelineDescriptor local3 = ref pipelineDescriptor;
            WGPUVertexState wgpuVertexState1 = new WGPUVertexState();
            wgpuVertexState1.module = vertexShader.Handle;
            ref WGPUVertexState local4 = ref wgpuVertexState1;
            WGPUStringView wgpuStringView = new WGPUStringView()
            {
                data = (sbyte*)numPtr,
                length = (UIntPtr)vertexEntryPoint.Length
            };
            local4.entryPoint = wgpuStringView;
            wgpuVertexState1.bufferCount = (UIntPtr)length;
            wgpuVertexState1.buffers = length > 0 ? vertexBufferLayoutPtr1 : (WGPUVertexBufferLayout*)null;
            WGPUVertexState wgpuVertexState2 = wgpuVertexState1;
            local3.vertex = wgpuVertexState2;
            pipelineDescriptor.primitive = Device.BuildPrimitive(primitive);
            pipelineDescriptor.multisample = Device.BuildMultisample(multisample);
            pipelineDescriptor.depthStencil = flag ? &depthStencilState : (WGPUDepthStencilState*)null;
            pipelineDescriptor.fragment = (WGPUFragmentState*)null;
            WGPURenderPipelineImpl* renderPipeline = WGPU.wgpuDeviceCreateRenderPipeline(this.Handle, &pipelineDescriptor);
            return (IntPtr)renderPipeline != IntPtr.Zero ? new RenderPipeline(renderPipeline) : throw new ResourceCreationException("RenderPipeline", "wgpuDeviceCreateRenderPipeline");
        }
    }

    private static WGPUDepthStencilState BuildDepthStencil(in DepthStencilState ds)
    {
        WGPUStencilFaceState stencilFaceState = new WGPUStencilFaceState()
        {
            compare = (WGPUCompareFunction)0,
            failOp = (WGPUStencilOperation)0,
            depthFailOp = (WGPUStencilOperation)0,
            passOp = (WGPUStencilOperation)0
        };
        return new WGPUDepthStencilState()
        {
            format = ds.Format,
            depthWriteEnabled = ds.DepthWriteEnabled ? (WGPUOptionalBool)1 : (WGPUOptionalBool)0,
            depthCompare = ds.DepthCompare,
            stencilFront = stencilFaceState,
            stencilBack = stencilFaceState,
            stencilReadMask = ds.StencilReadMask == 0U ? uint.MaxValue : ds.StencilReadMask,
            stencilWriteMask = ds.StencilWriteMask == 0U ? uint.MaxValue : ds.StencilWriteMask,
            depthBias = ds.DepthBias,
            depthBiasSlopeScale = ds.DepthBiasSlopeScale,
            depthBiasClamp = ds.DepthBiasClamp
        };
    }

    private static WGPUPrimitiveState BuildPrimitive(PrimitiveState p)
    {
        return new WGPUPrimitiveState()
        {
            topology = p.Topology,
            stripIndexFormat = p.StripIndexFormat,
            frontFace = p.FrontFace,
            cullMode = p.CullMode,
            unclippedDepth = p.UnclippedDepth ? 1u : 0u
        };
    }

    private static WGPUMultisampleState BuildMultisample(MultisampleState m)
    {
        return new WGPUMultisampleState()
        {
            count = m.Count,
            mask = m.Mask,
            alphaToCoverageEnabled = m.AlphaToCoverageEnabled ? 1u : 0u
        };
    }

    public unsafe ComputePipeline CreateComputePipeline(
      ShaderModule shader,
      scoped ReadOnlySpan<byte> entryPoint,
      PipelineLayout layout = default(PipelineLayout),
      scoped ReadOnlySpan<byte> label = default(ReadOnlySpan<byte>))
    {
        label = new ReadOnlySpan<byte>();
        fixed (byte* numPtr = &entryPoint.GetPinnableReference())
        fixed (byte* data = &label.GetPinnableReference())
        {
            WGPUComputePipelineDescriptor pipelineDescriptor = new WGPUComputePipelineDescriptor();
            pipelineDescriptor.label = WgpuDefaults.OptionalStringView(data, label.Length);
            pipelineDescriptor.layout = layout.Handle;
            ref WGPUComputePipelineDescriptor local1 = ref pipelineDescriptor;
            WGPUComputeState wgpuComputeState1 = new WGPUComputeState();
            wgpuComputeState1.module = shader.Handle;
            ref WGPUComputeState local2 = ref wgpuComputeState1;
            WGPUStringView wgpuStringView = new WGPUStringView()
            {
                data = (sbyte*)numPtr,
                length = (UIntPtr)entryPoint.Length
            };
            local2.entryPoint = wgpuStringView;
            WGPUComputeState wgpuComputeState2 = wgpuComputeState1;
            local1.compute = wgpuComputeState2;
            WGPUComputePipelineImpl* computePipeline = WGPU.wgpuDeviceCreateComputePipeline(this.Handle, &pipelineDescriptor);
            return (IntPtr)computePipeline != IntPtr.Zero ? new ComputePipeline(computePipeline) : throw new ResourceCreationException("ComputePipeline", "wgpuDeviceCreateComputePipeline");
        }
    }

    public unsafe CommandEncoder CreateCommandEncoder()
    {
        WGPUCommandEncoderImpl* commandEncoder = WGPU.wgpuDeviceCreateCommandEncoder(this.Handle, (WGPUCommandEncoderDescriptor*)null);
        return (IntPtr)commandEncoder != IntPtr.Zero ? new CommandEncoder(commandEncoder) : throw new ResourceCreationException("CommandEncoder", "wgpuDeviceCreateCommandEncoder");
    }

    public unsafe void Dispose()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            return;
        if ((IntPtr)this.Queue.Handle != IntPtr.Zero)
            WGPU.wgpuQueueRelease(this.Queue.Handle);
        WGPU.wgpuDeviceRelease(this._handle);
        this._handle = (WGPUDeviceImpl*)null;
        GC.SuppressFinalize((object)this);
    }

    unsafe ~Device()
    {
        if ((IntPtr)this._handle == IntPtr.Zero)
            return;
        if ((IntPtr)this.Queue.Handle != IntPtr.Zero)
            WGPU.wgpuQueueRelease(this.Queue.Handle);
        WGPU.wgpuDeviceRelease(this._handle);
    }
}
