using Data.Database;
using Data.Serializable.Converters;
using Newtonsoft.Json;
using Systems.LootSystem;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class LootTableRef : BaseRef<LootTable>
    {
        public LootTableRef(string key) : base(key, Databases.LootTables)
        {
        }
        
        private sealed class Converter : BaseRefConverter<LootTableRef, LootTable>
        {
            protected override LootTableRef CreateRef(string key) => new LootTableRef(key);
        }
    }
}