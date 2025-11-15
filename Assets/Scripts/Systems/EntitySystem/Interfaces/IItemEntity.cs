using Data.Models.Entities;
using Data.Models.Items;

namespace Systems.EntitySystem.Interfaces
{
    public interface IItemEntity : IMovingEntity
    {
        ItemInstance ItemInstance { get; }
        float LifeTime { get; }
        ItemEntityConfig Config { get; }
        void TryPickup(IPlayer player);
        
    }
}