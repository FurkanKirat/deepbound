using System;
using Core.Context.Registry;
using Data.RegistrySystem;
using Data.Serializable.Converters;
using Newtonsoft.Json;
using Systems.MovementSystem.Behaviors;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class MovementBehaviorRef : BaseFactoryRef<MovementBehaviorContext, IMovementBehavior>
    {
        private MovementBehaviorRef(string key) : base(key, Registries.MovementBehaviorFactory)
        {
        }
        
        private sealed class Converter : BaseFactoryRefConverter<MovementBehaviorRef, IMovementBehavior, MovementBehaviorContext>
        {
            public Converter() : base(key => new MovementBehaviorRef(key))
            {
            }
        }
    }
    
    
}