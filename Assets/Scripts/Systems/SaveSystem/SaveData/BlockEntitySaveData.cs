using System.Collections.Generic;
using Data.Models;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.BlockBehavior;

namespace Systems.SaveSystem.SaveData
{
    public class BlockEntitySaveData : ISaveData
    {
        public string BlockId { get; set; }
        public TilePosition Position;
        public int Width;
        public int Height;
        public List<BlockBehaviorSaveData> Behaviors;
    }
}