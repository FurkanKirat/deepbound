using Data.Models.Items;

namespace Core.Context.Spawn
{
    public class ItemEntitySpawnContext : BaseSpawnContext
    {
        public ItemInstance Item { get; set; }
    }
}