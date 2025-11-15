using Core.Context.Registry;
using Data.RegistrySystem;
using Data.Serializable.Converters;
using Interfaces;
using Newtonsoft.Json;
using Systems.EntitySystem.Interfaces;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class NpcBehaviorRef : BaseFactoryRef<NpcStateContext, IState<INpc>>
    {
        private NpcBehaviorRef(string key) : base(key, Registries.NpcStateFactory)
        {
        }
        private sealed class Converter : BaseFactoryRefConverter<NpcBehaviorRef, IState<INpc>, NpcStateContext>
        {
            public Converter() : base(key => new NpcBehaviorRef(key))
            {
            }
        }
        
    }
}