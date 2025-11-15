using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Systems.InventorySystem;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.Inventory
{
    [JsonConverter(typeof(Converter))]
    public class SlotCollectionSaveData : ISaveData
    {
        public SlotCollectionType Type;
        public SlotStorageSaveData SlotStorage;
        
        private class Converter : JsonConverter<SlotCollectionSaveData>
        {
            public override bool CanWrite => false;

            public override void WriteJson(JsonWriter writer, SlotCollectionSaveData value, JsonSerializer serializer)
            {
                
            }

            public override SlotCollectionSaveData ReadJson(JsonReader reader, Type objectType, SlotCollectionSaveData existingValue,
                bool hasExistingValue, JsonSerializer serializer)
            {
                JObject obj = JObject.Load(reader);

                var type = obj[nameof(Type)]?.ToObject<SlotCollectionType>();

                SlotCollectionSaveData result = type switch
                {
                    SlotCollectionType.Inventory => new InventorySaveData(),
                    SlotCollectionType.Equipment => new EquipmentSaveData(),
                    SlotCollectionType.Accessory => new AccessorySaveData(),
                    _ => throw new ArgumentOutOfRangeException()
                };

                serializer.Populate(obj.CreateReader(), result);
                return result;
            }
        }
    }
}