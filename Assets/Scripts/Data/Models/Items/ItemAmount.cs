using System;
using Data.Database;
using Interfaces;
using Newtonsoft.Json;

namespace Data.Models.Items
{
    [JsonConverter(typeof(Converter))]
    public class ItemAmount : IItem, IStringConvertible
    {
        public string Id { get; set; }
        public int Count { get; set; }

        private ItemData _itemData;
        public bool IsEmpty => ItemUtils.IsEmpty(Id);

        public ItemAmount(){}

        public ItemAmount(string id, int count)
        {
            Id = id;
            Count = count;
        }
        public ItemData ItemData
        {
            get
            {
                _itemData ??= Databases.Items[Id];
                return _itemData;
            }
        } 
        
        public string ToStringValue()
        {
            return Count == 1 ? Id : $"{Id}:{Count}";
        }

        public override string ToString() => ToStringValue();

        public static bool TryParse(string str, out ItemAmount item)
        {
            item = null;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            
            var parts = str.Split(':');
            int count = 1;
            if (parts.Length <= 2)
            {
                item = new ItemAmount
                {
                    Count = count,
                    Id = str
                };
                return true; 
            }

            var lastIndex = str.LastIndexOf(':');
            var id = str.Substring(0, lastIndex);
            var countStr = str.Substring(lastIndex + 1);
            if (int.TryParse(countStr, out count))
            {
                item = new ItemAmount
                {
                    Count = count,
                    Id = id
                };
                return true;
            }
            return false;
        }

        public ItemInstance ToItemInstance()
        {
            return ItemInstance.Create(Id, Count);
        }

        
        // Format: item:x:count AND item:x = item:x:1
        private sealed class Converter : JsonConverter<ItemAmount>
        {
            public override void WriteJson(JsonWriter writer, ItemAmount value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToStringValue());
            }

            public override ItemAmount ReadJson(JsonReader reader, Type objectType, ItemAmount existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                var str = (string)reader.Value;
                if (TryParse(str, out ItemAmount item))
                    return item;
                
                throw new JsonSerializationException($"Wrong Item Amount format: {str}.");
            }
        }
    }
    
    

}