using Core.Context.Registry;
using Data.Models.Blocks.Behaviors;
using Data.RegistrySystem;
using Data.Serializable.Converters;
using Newtonsoft.Json;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class BlockBehaviorRef : BaseFactoryRef<BlockBehaviorContext, IBlockBehavior>
    {
        private BlockBehaviorRef(string key) : base(key, Registries.BlockBehaviorFactory)
        {
        }
        
        private sealed class Converter : BaseFactoryRefConverter<BlockBehaviorRef, IBlockBehavior, BlockBehaviorContext>
        {
            public Converter() : base(key => new BlockBehaviorRef(key))
            {
            }
        }
        
    }
}