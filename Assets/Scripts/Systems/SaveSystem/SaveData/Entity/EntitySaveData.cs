using System;
using Data.Models;
using Data.Serializable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Systems.EntitySystem;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.Entity
{
    [JsonConverter(typeof(Converter))]
    public class EntitySaveData : ISaveData
    {
        public int Id;
        public EntityType Type;
        public WorldPosition Position;
        public Float2 Velocity;
        public CharacterState CharacterState;
        public CooldownSaveData Cooldowns;
        public EffectsSaveData Effects;

        public EntitySaveData()
        {
            
        }

        public EntitySaveData(EntitySaveData other)
        {
            Id = other.Id;
            Type = other.Type;
            Position = other.Position;
            Velocity = other.Velocity;
            CharacterState = other.CharacterState;
            Cooldowns = other.Cooldowns;
            Effects = other.Effects;
        }
        private sealed class Converter : JsonConverter<EntitySaveData>
        {
            public override bool CanWrite => false;
            public override void WriteJson(JsonWriter writer, EntitySaveData value, JsonSerializer serializer)
            {
                
            }

            public override EntitySaveData ReadJson(JsonReader reader, Type objectType, EntitySaveData existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                JObject obj = JObject.Load(reader);

                var type = obj[nameof(Type)]?.ToObject<EntityType>();

                EntitySaveData result = type switch
                {
                    EntityType.Player => new PlayerSaveData(),
                    EntityType.Enemy => new EnemySaveData(),
                    EntityType.Item => new ItemEntitySaveData(),
                    EntityType.Projectile => new ProjectileSaveData(),
                    _ => new EntitySaveData()
                };

                serializer.Populate(obj.CreateReader(), result);
                return result;
            }
        }
    }
    
    
}