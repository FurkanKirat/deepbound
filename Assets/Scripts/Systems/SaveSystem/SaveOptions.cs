using System;

namespace Systems.SaveSystem
{
    [Flags]
    public enum SaveOptions : byte
    {
        None = 0,
        Compress = 1 << 1,
        Hash = 1 << 2,
    }
}