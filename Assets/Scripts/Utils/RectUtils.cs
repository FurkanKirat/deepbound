using Data.Serializable;

namespace Utils
{
    public static class RectUtils
    {
        public static Int2 GetNextCoordinate(int current, int cellWidth, int cellHeight, int cellCountPerY)
        {
            var x = current % cellCountPerY;
            var y = current / cellCountPerY;
            if (x == cellCountPerY - 1)
            {
                return new Int2(0, (y + 1) * cellHeight) ;
            }
            
            return new Int2((x + 1) * cellWidth, y * cellHeight);
        }
        
        public static Int2 GetCoordinate(int current, int cellCountPerY)
        {
            var x = current % cellCountPerY;
            var y = current / cellCountPerY;
            return new Int2(x, y);
        }

        public static bool IsInsideRect(this IntRect intRect, Int2 position)
        {
            return position.x >= intRect.x &&
                   position.y >= intRect.y &&
                   intRect.x + intRect.width >= position.x && 
                   intRect.y + intRect.height >= position.y;
        }
    }
}