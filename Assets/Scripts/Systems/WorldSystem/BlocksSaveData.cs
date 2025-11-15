using System.Collections.Generic;
using Systems.SaveSystem.SaveData;

namespace Systems.WorldSystem
{
    public class BlocksSaveData
    {
        public WorldGrid<ushort> WorldGrid;
        public List<BlockEntitySaveData> BlockEntities;
    }
}