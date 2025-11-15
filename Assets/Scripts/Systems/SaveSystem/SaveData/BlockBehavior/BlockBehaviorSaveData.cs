using System;
using Generated.Ids;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.BlockBehavior
{
    [JsonConverter(typeof(Converter))]
    public class BlockBehaviorSaveData : ISaveData
    {
        public string BehaviorId;
        
        private sealed class Converter : JsonConverter<BlockBehaviorSaveData>
        {
            public override bool CanWrite => false;
            public override void WriteJson(JsonWriter writer, BlockBehaviorSaveData value, JsonSerializer serializer)
            {
                
            }

            public override BlockBehaviorSaveData ReadJson(JsonReader reader, Type objectType, BlockBehaviorSaveData existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                
                JObject obj = JObject.Load(reader);

                var type = obj[nameof(BehaviorId)]?.ToObject<string>();

                BlockBehaviorSaveData result = type switch
                {
                    BlockBehaviorIds.Chest => new ChestBehaviorSaveData(),
                    BlockBehaviorIds.Crop => new CropBehaviorSaveData(),
                    BlockBehaviorIds.Portal => new PortalBehaviorSaveData(),
                    _ => new BlockBehaviorSaveData(),
                };

                serializer.Populate(obj.CreateReader(), result);
                return result;
            }
        }
    }
}