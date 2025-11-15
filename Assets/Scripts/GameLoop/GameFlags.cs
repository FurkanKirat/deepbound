using System;

namespace GameLoop
{
    [Flags]
    public enum GameFlags
    {
        None = 0,
        Paused = 1 << 0,
        UIBlocking = 1 << 1,
        Cutscene = 1 << 2,
        Dialogue = 1 << 3
    }
}