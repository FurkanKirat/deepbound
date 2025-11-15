using Data.Database;
using Data.Serializable.Converters;
using Newtonsoft.Json;
using Systems.SpawnSystem;

namespace Data.Models.References
{
    
    [JsonConverter(typeof(Converter))]
    public class EnemySpawnTableRef : BaseRef<EnemySpawnTable>
    {
        public EnemySpawnTableRef(string key) : base(key, Databases.EnemySpawnTables)
        {
        }
    
        private sealed class Converter : BaseRefConverter<EnemySpawnTableRef, EnemySpawnTable>
        {
            protected override EnemySpawnTableRef CreateRef(string key) => new (key);
        }
    }
    
}