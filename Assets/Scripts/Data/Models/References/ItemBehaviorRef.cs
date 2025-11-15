using Data.Models.Items.Behaviors;
using Data.RegistrySystem;
using Data.Serializable.Converters;
using Newtonsoft.Json;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class ItemBehaviorRef : BaseRef<IItemBehavior>
    {
        public ItemBehaviorRef(string key) : base(key, Registries.ItemBehaviors)
        {
        }
        
        private sealed class Converter : BaseRefConverter<ItemBehaviorRef, IItemBehavior>
        {
            protected override ItemBehaviorRef CreateRef(string key) => new ItemBehaviorRef(key);
        }
        
    }
}