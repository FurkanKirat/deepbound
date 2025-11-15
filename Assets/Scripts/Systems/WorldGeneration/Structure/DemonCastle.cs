using Core.Context;
using Data.Models.Blocks;
using Data.Models.Generation;
using Generated.Ids;
using Systems.WorldGeneration.Structure.Generated;
using UnityEngine;
using Utils;
using RangeInt = Data.Serializable.RangeInt;

namespace Systems.WorldGeneration.Structure
{
    public class DemonCastle : IStructurePlacer
    {
        private const int RoomCount = 13;
        private GeneratedDemonCastle _generated;
        public bool CanPlace(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context)
        {
            _generated = new GeneratedDemonCastle
            {
                RoomWidths = new int[RoomCount],
                RoomHeights = new int[RoomCount]
            };
            var maxWidth = 0;
            var totalHeight = 0;

            for (var i = 0; i < RoomCount; i++)
            {
                var roomWidthRange = new RangeInt(15, 20);
                var roomHeightRange = new RangeInt(10, 15);
            
                var roomWidth = roomWidthRange.Roll(context.Random);
                var roomHeight = roomHeightRange.Roll(context.Random);
                _generated.RoomWidths[i] = roomWidth;
                _generated.RoomHeights[i] = roomHeight;
                maxWidth = Mathf.Max(maxWidth, roomWidth);
                totalHeight += roomHeight;
            }
            
            int left = baseX;
            int right = baseX + maxWidth - 1;
            int bottom = baseY;
            int top = baseY + totalHeight - 1;

            return left >= 0 && right < context.Width && bottom >= 0 && top < context.Height;
        }

        public void PlaceStructure(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context)
        {
            var y = 0;
            for (int i = 0; i < RoomCount; i++)
            {
                var roomWidth = _generated.RoomWidths[i];
                var roomHeight = _generated.RoomHeights[i];
                var blocks = ShapeHelper.GetRectangleEdges(baseX, y, roomWidth, roomHeight);
                
                foreach (var block in blocks)
                {
                    if (!context.Blocks.IsInBounds(block.x, block.y))
                        continue;
                    context.Blocks[block.x, block.y] = Block.CreateMaster(BlockIds.HellBrick);
                }
                y += roomHeight;
            }
            
        }
    }
}