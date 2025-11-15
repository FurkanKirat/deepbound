using System;
using Core;
using Core.Events;
using Data.Database;
using Data.Models;
using Data.Models.Dimensions;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;

namespace Systems.WorldSystem
{
    public class LayerManager : ISaveable<LayersSaveData>
    {
        private WorldLayer _lastLayer;
        private int _lastPlayerY;
        private readonly int[] _surfaceYPerX;
        private readonly DimensionData _dimension;
        
        private LayerManager(string dimensionId, int[] surfaceYPerX)
        {
            _dimension = Databases.Dimensions[dimensionId];
            _surfaceYPerX = surfaceYPerX;
            _lastLayer = null;
        }

        public static LayerManager Create(string dimensionId, int[] surfaceYPerX)
        {
            return new LayerManager(dimensionId, surfaceYPerX);
        }

        public static LayerManager Load(string dimensionId, LayersSaveData saveData)
        {
            return new LayerManager(dimensionId, saveData.SurfaceYPerX);
        }

        public void UpdatePlayerLayer(WorldPosition playerPos)
        {
            if (Math.Abs(playerPos.y - _lastPlayerY) > 1)
            {
                var playerTilePos = playerPos.ToTilePosition();
                var newLayer = GetLayerForPosition(playerTilePos);
                if (newLayer != _lastLayer)
                {
                    _lastLayer = newLayer;
                    GameEventBus.Publish(new WorldLayerChangedEvent(_lastLayer));
                }
                _lastPlayerY = playerTilePos.Y;
            }
        }

        public WorldLayer GetLayerForPosition(TilePosition pos)
        {
            foreach (var layer in _dimension.Layers)
            {
                int y = 0;
                if (_surfaceYPerX != null) 
                    y = _surfaceYPerX[pos.X];
                
                if (layer.Matches(pos, y))
                    return layer;
            }
            return null;
        }
        
        public int GetYPerColumn(int x) => _surfaceYPerX[x];

        public LayersSaveData ToSaveData()
        {
            return new LayersSaveData
            {
                SurfaceYPerX = _surfaceYPerX
            };
        }
    }

}