using Brew.Wgpu.Native;
using System.Runtime.CompilerServices;

#nullable disable
namespace Brew.Wgpu;

public static class TextureFormats
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (uint w, uint h) BlockDimensions(this WGPUTextureFormat f)
    {
        (uint, uint) valueTuple;
        if ((int)f >= 50)
        {
            if ((int)f > 73)
            {
                switch (((int)f - 74))
                {
                    case 0:
                    case 1:
                        valueTuple = (4U, 4U);
                        goto label_21;
                    case 2:
                    case 3:
                        valueTuple = (5U, 4U);
                        goto label_21;
                    case 4:
                    case 5:
                        valueTuple = (5U, 5U);
                        goto label_21;
                    case 6:
                    case 7:
                        valueTuple = (6U, 5U);
                        goto label_21;
                    case 8:
                    case 9:
                        valueTuple = (6U, 6U);
                        goto label_21;
                    case 10:
                    case 11:
                        valueTuple = (8U, 5U);
                        goto label_21;
                    case 12:
                    case 13:
                        valueTuple = (8U, 6U);
                        goto label_21;
                    case 14:
                    case 15:
                        valueTuple = (8U, 8U);
                        goto label_21;
                    case 16 /*0x10*/:
                    case 17:
                        valueTuple = (10U, 5U);
                        goto label_21;
                    case 18:
                    case 19:
                        valueTuple = (10U, 6U);
                        goto label_21;
                    case 20:
                    case 21:
                        valueTuple = (10U, 8U);
                        goto label_21;
                    case 22:
                    case 23:
                        valueTuple = (10U, 10U);
                        goto label_21;
                    case 24:
                    case 25:
                        valueTuple = (12U, 10U);
                        goto label_21;
                    case 26:
                    case 27:
                        valueTuple = (12U, 12U);
                        goto label_21;
                }
            }
            else
            {
                valueTuple = (4U, 4U);
                goto label_21;
            }
        }
        else if (false)
        {
            valueTuple = (0U, 0U);
            goto label_21;
        }
        valueTuple = (1U, 1U);
    label_21:
        return valueTuple;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint BytesPerBlock(this WGPUTextureFormat f)
    {
        uint num;
        if ((int)f <= 63 /*0x3F*/)
        {
            if ((int)f >= 52)
            {
                if ((int)f > 55)
                {
                    if (((int)f - 56) > 1)
                    {
                        num = 16U /*0x10*/;
                        goto label_20;
                    }
                }
                else
                {
                    num = 16U /*0x10*/;
                    goto label_20;
                }
            }
            else
            {
                switch (((int)f - 1))
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 43:
                        num = 1U;
                        goto label_20;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 44:
                        num = 2U;
                        goto label_20;
                    case 13:
                    case 14:
                    case 15:
                    case 16 /*0x10*/:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                    case 30:
                    case 31 /*0x1F*/:
                    case 47:
                        num = 4U;
                        goto label_20;
                    case 32 /*0x20*/:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                        num = 8U;
                        goto label_20;
                    case 40:
                    case 41:
                    case 42:
                        num = 16U /*0x10*/;
                        goto label_20;
                    case 45:
                    case 46:
                    case 48 /*0x30*/:
                        num = 0U;
                        goto label_20;
                    case 49:
                    case 50:
                        break;
                    default:
                        goto label_19;
                }
            }
            num = 8U;
            goto label_20;
        }
        if ((int)f <= 101)
        {
            switch (((int)f - 64) /*0x40*/)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 6:
                case 7:
                    num = 8U;
                    goto label_20;
                case 4:
                case 5:
                case 8:
                case 9:
                    num = 16U /*0x10*/;
                    goto label_20;
                default:
                    num = 16U /*0x10*/;
                    goto label_20;
            }
        }
    label_19:
        num = 0U;
    label_20:
        return num;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FormatAspect Aspect(this WGPUTextureFormat f)
    {
        FormatAspect formatAspect;
        if (f != null)
        {
            switch (((int)f - 44))
            {
                case 0:
                    formatAspect = FormatAspect.Stencil;
                    break;
                case 1:
                case 2:
                case 4:
                    formatAspect = FormatAspect.Depth;
                    break;
                case 3:
                case 5:
                    formatAspect = FormatAspect.Depth | FormatAspect.Stencil;
                    break;
                default:
                    formatAspect = FormatAspect.Color;
                    break;
            }
        }
        else
            formatAspect = FormatAspect.None;
        return formatAspect;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSRGB(this WGPUTextureFormat f)
    {
        if ((int)f <= 28)
        {
            if ((int)f != 23 && (int)f != 28)
                goto label_6;
        }
        else
        {
            switch (((int)f - 51))
            {
                case 0:
                case 2:
                case 4:
                    break;
                case 1:
                case 3:
                    goto label_6;
                default:
                    switch (((int)f - 63) /*0x3F*/)
                    {
                        case 0:
                        case 2:
                        case 4:
                        case 6:
                            break;
                        case 1:
                        case 3:
                        case 5:
                            goto label_6;
                        default:
                            switch (((int)f - 75))
                            {
                                case 0:
                                case 2:
                                case 4:
                                case 6:
                                case 8:
                                case 10:
                                case 12:
                                case 14:
                                case 16 /*0x10*/:
                                case 18:
                                case 20:
                                case 22:
                                case 24:
                                case 26:
                                    break;
                                default:
                                    goto label_6;
                            }
                            break;
                    }
                    break;
            }
        }
        bool flag = true;
        goto label_7;
    label_6:
        flag = false;
    label_7:
        return flag;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCompressed(this WGPUTextureFormat f) => (int)f >= 50 && (int)f <= 101;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDepthOrStencil(this WGPUTextureFormat f)
    {
        return (f.Aspect() & (FormatAspect.Depth | FormatAspect.Stencil)) != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetLinearCompanion(this WGPUTextureFormat f, out WGPUTextureFormat linear)
    {
        WGPUTextureFormat wgpuTextureFormat;
        if ((int)f <= 28)
        {
            if ((int)f != 23)
            {
                if ((int)f == 28)
                {
                    wgpuTextureFormat = WGPUTextureFormat.BGRA8Unorm;
                    goto label_30;
                }
            }
            else
            {
                wgpuTextureFormat = WGPUTextureFormat.RGBA8Unorm;
                goto label_30;
            }
        }
        else
        {
            switch (((int)f - 51))
            {
                case 0:
                    wgpuTextureFormat = WGPUTextureFormat.BC1RGBAUnorm;
                    goto label_30;
                case 1:
                case 3:
                    break;
                case 2:
                    wgpuTextureFormat = WGPUTextureFormat.BC2RGBAUnorm;
                    goto label_30;
                case 4:
                    wgpuTextureFormat = WGPUTextureFormat.BC3RGBAUnorm;
                    goto label_30;
                default:
                    switch (((int)f - 63) /*0x3F*/)
                    {
                        case 0:
                            wgpuTextureFormat = WGPUTextureFormat.BC7RGBAUnorm;
                            goto label_30;
                        case 1:
                        case 3:
                        case 5:
                            break;
                        case 2:
                            wgpuTextureFormat = WGPUTextureFormat.ETC2RGB8Unorm;
                            goto label_30;
                        case 4:
                            wgpuTextureFormat = WGPUTextureFormat.ETC2RGB8A1Unorm;
                            goto label_30;
                        case 6:
                            wgpuTextureFormat = WGPUTextureFormat.ETC2RGBA8Unorm;
                            goto label_30;
                        default:
                            switch (((int)f - 75))
                            {
                                case 0:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC4x4Unorm;
                                    goto label_30;
                                case 2:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC5x4Unorm;
                                    goto label_30;
                                case 4:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC5x5Unorm;
                                    goto label_30;
                                case 6:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC6x5Unorm;
                                    goto label_30;
                                case 8:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC6x6Unorm;
                                    goto label_30;
                                case 10:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC8x5Unorm;
                                    goto label_30;
                                case 12:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC8x6Unorm;
                                    goto label_30;
                                case 14:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC8x8Unorm;
                                    goto label_30;
                                case 16 /*0x10*/:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC10x5Unorm;
                                    goto label_30;
                                case 18:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC10x6Unorm;
                                    goto label_30;
                                case 20:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC10x8Unorm;
                                    goto label_30;
                                case 22:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC10x10Unorm;
                                    goto label_30;
                                case 24:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC12x10Unorm;
                                    goto label_30;
                                case 26:
                                    wgpuTextureFormat = WGPUTextureFormat.ASTC12x12Unorm;
                                    goto label_30;
                            }
                            break;
                    }
                    break;
            }
        }
        wgpuTextureFormat = WGPUTextureFormat.Undefined;
    label_30:
        linear = wgpuTextureFormat;
        return (int)linear != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUTextureSampleType DefaultSampleType(this WGPUTextureFormat f)
    {
        WGPUTextureSampleType textureSampleType;
        switch ((int)f)
        {
            case 0:
                textureSampleType = WGPUTextureSampleType.Undefined;
                break;
            case 3:
            case 7:
            case 12:
            case 15:
            case 19:
            case 25:
            case 29:
            case 34:
            case 38:
            case 42:
            case 44:
                textureSampleType = WGPUTextureSampleType.Uint;
                break;
            case 4:
            case 8:
            case 13:
            case 16 /*0x10*/:
            case 20:
            case 26:
            case 35:
            case 39:
            case 43:
                textureSampleType = WGPUTextureSampleType.Sint;
                break;
            case 14:
            case 33:
            case 41:
                textureSampleType = WGPUTextureSampleType.UnfilterableFloat;
                break;
            case 45:
            case 46:
            case 47:
            case 48 /*0x30*/:
            case 49:
                textureSampleType = WGPUTextureSampleType.Depth;
                break;
            default:
                textureSampleType = WGPUTextureSampleType.Float;
                break;
        }
        return textureSampleType;
    }
}
