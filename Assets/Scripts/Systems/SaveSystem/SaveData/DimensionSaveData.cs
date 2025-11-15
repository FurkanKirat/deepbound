using System.Collections.Generic;
using Data.Models;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Entity;
using Systems.WorldSystem;

namespace Systems.SaveSystem.SaveData
{
    public class DimensionSaveData : ISaveData
    { 
        public string DimensionId { get; set; }
        public BlocksSaveData BlocksSaveData { get; set; }
        public List<EntitySaveData> Entities { get; set; }
        public WorldPosition PlayerSpawn { get; set; }
        public LayersSaveData LayersSaveData { get; set; }
        public MinimapSaveData MinimapSaveData { get; set; }
    }
}