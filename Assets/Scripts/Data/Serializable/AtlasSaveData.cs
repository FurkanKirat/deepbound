using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data.Serializable
{
    [JsonConverter(typeof(AtlasSaveDataConverter))]
    public class AtlasSaveData
    {
        public Int2[] Positions;
        public Int2 Size;

        public AtlasSaveData(Int2[] positions, Int2 size)
        {
            Positions = positions;
            Size = size;
        }
        
        public class AtlasSaveDataConverter : JsonConverter<AtlasSaveData>
        {
            public override void WriteJson(JsonWriter writer, AtlasSaveData value, JsonSerializer serializer)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Positions");
                serializer.Serialize(writer, value.Positions);
                
                if (!(value.Size.x == 1 && value.Size.y == 1))
                {
                    writer.WritePropertyName("Size");
                    serializer.Serialize(writer, value.Size);
                }

                writer.WriteEndObject();
            }

            public override AtlasSaveData ReadJson(JsonReader reader, Type objectType, AtlasSaveData existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                JObject obj = JObject.Load(reader);

                var positions = obj["Positions"]?.ToObject<Int2[]>(serializer) ?? Array.Empty<Int2>();
            
                Int2 size = obj["Size"]?.ToObject<Int2>(serializer) ?? new Int2(1, 1);

                return new AtlasSaveData(positions, size);
            }
        }
    }
}