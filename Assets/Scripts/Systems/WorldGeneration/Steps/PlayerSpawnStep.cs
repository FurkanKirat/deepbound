using Core.Context;
using Data.Database;
using Data.Models;
using Data.Models.Blocks;
using Generated.Ids;
using Systems.Physics.Colliders;
using Systems.WorldSystem;
using UnityEngine;
using Utils;

namespace Systems.WorldGeneration.Steps
{
    public class PlayerSpawnStep : IMapGenerationStep
    {
        public void Apply(MapGenerationContext context)
        {
            var playerSpawnConfig =
                context.WorldGenDatabase.PlayerSpawnConfig[context.DimensionGenerationData.PlayerSpawnConfigFile];

            var mapWidth = context.Width;
            var mapHeight = context.Height;
            
            var minX = playerSpawnConfig.MinX.Evaluate(mapWidth);
            var maxX = playerSpawnConfig.MaxX.Evaluate(mapWidth);
            var minY = playerSpawnConfig.MinY.Evaluate(mapHeight);
            var maxY = playerSpawnConfig.MaxY.Evaluate(mapHeight);
            
            var size = Configs.PlayerConfig.Size;
            GameLogger.Log(
                $"[PlayerSpawnStep] Spawn bounds: X[{minX}, {maxX}], Y[{minY}, {maxY}], Attempts={playerSpawnConfig.MaxAttempts}, SurfaceOnly={playerSpawnConfig.SurfaceOnly}",
                nameof(PlayerSpawnStep));
            
            int x = context.Random.Next(minX, maxX + 1);
            int y = context.Random.Next(minY, maxY + 1);

            if (playerSpawnConfig.SurfaceOnly)
            {
                y = context.SurfaceYPerColumn[Mathf.Clamp(x, 0, mapWidth - 1)];
            }
            
            context.PlayerSpawnPosition = ArrangeSpawn(x, y, context.Blocks).ToWorldPosition() + size / 2;
            GameLogger.Log(
                $"[PlayerSpawnStep] SUCCESS: Spawn set at worldPos={context.PlayerSpawnPosition} (tile=({x},{y + 1}))",
                nameof(PlayerSpawnStep));
        }


        private static TilePosition ArrangeSpawn(int x, int y, WorldGrid<Block> blocks)
        {
            var size = Configs.PlayerConfig.Size;
            var collider = new AABBCollider(new WorldPosition(x,y), size);
            var bounds = collider.Bounds.ToBounds2DIntInclusive();
            
            for(int spawnX = bounds.MinX; spawnX <= bounds.MaxX; spawnX++)
            for(int spawnY = bounds.MinY; spawnY <= bounds.MaxY; spawnY++)
            {
                bool inBounds = blocks.TryGetBlock(spawnX, spawnY, out var blockHere);
                bool isAir = inBounds && blockHere.IsAir();
                
                if (!isAir)
                    blocks.ClearBlock(spawnX, spawnY);
                
                if (spawnY == bounds.MinY && !(blocks.TryGetBlock(spawnX, spawnY - 1, out var blockBelow)
                    && !blockBelow.IsSolid()))
                {
                    blocks[spawnX, spawnY - 1] = Block.CreateMaster(BlockIds.Grass);
                }
            }
            
            return new TilePosition(bounds.MinX, bounds.MinY);
        }
    }
}
