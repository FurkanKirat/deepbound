using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.Input
{
    public class ActionBindingSaveData : ISaveData
    {
        public InputBindingSaveData Primary { get; set; }
        public InputBindingSaveData Secondary { get; set; }
    }
}