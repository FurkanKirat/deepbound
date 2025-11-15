using System.Collections.Generic;
using Data.Models.Items;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData
{
    public class ItemSaveData : ISaveData
    {
        public ItemAmount ItemAmount;
        public List<ItemModifier> Modifiers;
    }
}