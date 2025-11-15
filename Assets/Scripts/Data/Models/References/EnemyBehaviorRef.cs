using Core.Context.Registry;
using Data.RegistrySystem;
using Data.Serializable.Converters;
using Interfaces;
using Newtonsoft.Json;
using Systems.EntitySystem.Interfaces;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class EnemyBehaviorRef : BaseFactoryRef<EnemyStateContext, IState<IEnemy>>
    {

        private EnemyBehaviorRef(string key) : base(key, Registries.EnemyBehaviorFactory)
        {
        }
        
        private sealed class Converter : BaseFactoryRefConverter<EnemyBehaviorRef, IState<IEnemy>, EnemyStateContext>
        {
            public Converter() : base(key => new EnemyBehaviorRef(key))
            {
            }
        }
        
    }
}