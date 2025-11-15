using System.Collections.Generic;
using Constants;
using Data.Models;

namespace Systems.Physics
{
    public struct Bounds2DInt
    {
        public int MinX;
        public int MinY;
        public int MaxX;
        public int MaxY;

        public Bounds2DInt(int minX, int minY, int maxX, int maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public static Bounds2DInt FromTile(TilePosition position)
        {
            return new Bounds2DInt
            {
                MinX = position.X,
                MinY = position.Y,
                MaxX = position.X + (int)TileConstants.TileSize.x,
                MaxY = position.Y + (int)TileConstants.TileSize.y
            };
        }

        public int Width => MaxX - MinX + 1;
        public int Height => MaxY - MinY + 1;

        public IEnumerable<TilePosition> GetTiles()
        {
            for (int y = MinY; y <= MaxY; y++)
            for (int x = MinX; x <= MaxX; x++)
                yield return new TilePosition(x, y);
        }

        public bool Contains(TilePosition pos)
        {
            return pos.X >= MinX && pos.X <= MaxX &&
                   pos.Y >= MinY && pos.Y <= MaxY;
        }
        
        public override string ToString()
        {
            return $"Bounds2DInt(({MinX},{MinY}) to ({MaxX},{MaxY}))";
        }

    }

}