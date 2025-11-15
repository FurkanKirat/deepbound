using System;

namespace Systems.WorldGeneration.Structure
{
    [Flags]
    public enum HeightLayer : byte
    {
        None = 0,
        Surface = 1 << 0,
        Underground = 1 << 1,
    }
}