using Core;
using Core.Events;
using Data.Models;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;
using UnityEngine;

namespace Systems.WorldSystem
{
    public class MinimapDiscovery : ISaveable<MinimapSaveData>
    {
        private readonly WorldGrid<bool> _chunkDiscovered;
        public const int ChunkSize = 16;

        private MinimapDiscovery(WorldGrid<bool> chunkDiscovered)
        {
            _chunkDiscovered = chunkDiscovered;
        }

        public static MinimapDiscovery Create(int mapWidth, int mapHeight)
        {
            int chunkWidthCount = Mathf.CeilToInt((float)mapWidth / ChunkSize);
            int chunkHeightCount = Mathf.CeilToInt((float)mapHeight / ChunkSize);
            
            var chunks = new WorldGrid<bool>(chunkWidthCount, chunkHeightCount);
            return new MinimapDiscovery(chunks);
        }

        public static MinimapDiscovery Load(MinimapSaveData saveData)
        {
            return new MinimapDiscovery(saveData.ChunksDiscovered);
        }
        
        public void Discover(TilePosition blockPos)
        {
            int cx = blockPos.X / ChunkSize;
            int cy = blockPos.Y / ChunkSize;
            if (!_chunkDiscovered[cx, cy])
            {
                _chunkDiscovered[cx, cy] = true;
                GameEventBus.Publish(new MinimapChunkDiscoveredEvent(cx,cy));
            }
                
        }
        
        public bool IsBlockDiscovered(int x, int y)
        {
            int cx = x / ChunkSize;
            int cy = y / ChunkSize;
            return _chunkDiscovered[cx, cy];
        }

        public bool IsChunkDiscovered(int x, int y)
            => _chunkDiscovered[x, y];
        
        public MinimapSaveData ToSaveData()
        {
            return new MinimapSaveData
            {
                ChunksDiscovered = _chunkDiscovered
            };
        }
    }
}