using System.Collections.Generic;
using Constants;
using Constants.Paths;
using Data.Serializable;
using Utils;
using UnityEngine;

namespace Data.Database
{
    public static class AtlasUVIndex
    {
        private static Dictionary<string, BlockAtlasSaveData> _uvMap = new();
        private static Rect _defaultRect;
        public static void SaveUVData(Dictionary<string, BlockAtlasSaveData> map)
        {
            _uvMap = map;
            ResourcesHelper.SaveToJson(AtlasPaths.UvJsonResourcePath, map);
        }
        
        public static void LoadUVData()
        {
            var serializableMap = ResourcesHelper.LoadJson<Dictionary<string,BlockAtlasSaveData>>(AtlasPaths.UvJsonResourcePathRelative);
            _uvMap = serializableMap;
            _defaultRect = GetUV(Defaults.Block, 0,0,0);
        }
        
        public static Rect GetUV(string textureId, int frame, byte offsetX, byte offsetY)
        {
            var id = textureId.Split('_')[0];
            if (!_uvMap.TryGetValue(id, out var blockAtlasData) || 
                blockAtlasData.Frames == null || 
                blockAtlasData.Frames.Length == 0)
                return _defaultRect;

            frame = Mathf.Clamp(frame, 0, blockAtlasData.Frames.Length - 1);
            var atlasSaveData = blockAtlasData.Frames[frame];

            if (atlasSaveData.Positions == null || atlasSaveData.Positions.Length == 0)
                return _defaultRect;

            int index = offsetY * atlasSaveData.Size.x + offsetX;
            if (index < 0 || index >= atlasSaveData.Positions.Length)
                return _defaultRect;

            var rectInt = atlasSaveData.Positions[index];
            return new Rect(
                rectInt.x * TileAtlasConstants.GridSize,
                rectInt.y * TileAtlasConstants.GridSize,
                TileAtlasConstants.GridSize,
                TileAtlasConstants.GridSize
            );
        }

        
    }
    
}