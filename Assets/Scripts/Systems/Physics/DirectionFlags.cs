namespace Systems.Physics
{
    [System.Flags]
    public enum DirectionFlags : byte
    {
        None  = 0,
        Up    = 1 << 0,
        Down  = 1 << 1,
        Left  = 1 << 2,
        Right = 1 << 3,

        TopLeft     = 1 << 4,
        TopRight    = 1 << 5,
        BottomLeft  = 1 << 6,
        BottomRight = 1 << 7,

        Horizontal = Left | Right,
        Vertical = Up | Down,
        
        AllEdges = Up | Down | Left | Right,
        AllCorners = TopLeft | TopRight | BottomLeft | BottomRight,
        
        All = AllEdges | AllCorners
    }

}