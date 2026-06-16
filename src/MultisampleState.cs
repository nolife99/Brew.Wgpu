#nullable disable
namespace Brew.Wgpu;

public struct MultisampleState
{
    public uint Count;
    public uint Mask;
    public bool AlphaToCoverageEnabled;

    public static MultisampleState Default
    {
        get
        {
            return new MultisampleState()
            {
                Count = 1,
                Mask = uint.MaxValue
            };
        }
    }
}
