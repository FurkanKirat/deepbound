using Core.Context.Registry;
using Data.RegistrySystem;
using Data.Serializable.Converters;
using Newtonsoft.Json;
using Systems.CombatSystem.Behaviors;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class AttackBehaviorRef : BaseFactoryRef<AttackBehaviorContext, IAttackBehavior>
    {
        private AttackBehaviorRef(string key) : base(key, Registries.AttackBehaviorFactory)
        {
        }
        
        private sealed class Converter : BaseFactoryRefConverter<AttackBehaviorRef, IAttackBehavior, AttackBehaviorContext>
        {
            public Converter() : base(key => new AttackBehaviorRef(key))
            {
            }
        }
    }
    
}