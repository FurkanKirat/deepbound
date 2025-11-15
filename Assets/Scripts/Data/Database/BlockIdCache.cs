using System.Collections.Generic;
using Data.Models.Blocks;
using Generated.Ids;

namespace Data.Database
{
    public static class BlockIdCache
    {
        private static readonly Dictionary<string, ushort> StringToUshort = new();
        private static readonly Dictionary<ushort, BlockData> UshortToBlockData = new();

        public const ushort AirId = 0;
        
        public static BlockData GetBlockData (ushort blockId) 
            => UshortToBlockData.GetValueOrDefault(blockId);
        
        public static ushort GetUshort(string blockId) => 
            StringToUshort.GetValueOrDefault(blockId, (ushort)0);

        public static void LoadAll()
        {
            ushort counter = 1;
            UshortToBlockData[AirId] = Databases.Blocks[BlockIds.Air];
            StringToUshort[BlockIds.Air] = AirId;
            foreach (var blockData in Databases.Blocks.All)
            {
                if (blockData.Id == BlockIds.Air) continue;
                UshortToBlockData[counter] = blockData;
                StringToUshort[blockData.Id] = counter;
                counter++;
            }
            
        }
    }
}