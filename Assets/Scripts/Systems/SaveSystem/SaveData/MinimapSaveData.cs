using Systems.SaveSystem.Interfaces;
using Systems.WorldSystem;

namespace Systems.SaveSystem.SaveData
{
    public class MinimapSaveData : ISaveData
    {
        public WorldGrid<bool> ChunksDiscovered;
    }
}