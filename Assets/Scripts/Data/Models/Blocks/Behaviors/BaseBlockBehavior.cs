using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Systems.WorldSystem;

namespace Data.Models.Blocks.Behaviors
{
    public abstract class BaseBlockBehavior : IBlockBehavior
    {
        public abstract string BehaviorId { get; }
        public TilePosition Position { get; }

        protected BaseBlockBehavior(TilePosition position)
        {
            Position = position;
        }
        public virtual void Interact(IPlayer player, World world) {}
        public virtual void Tick(float timeInterval, TickContext ctx) {}
        public virtual void OnCollisionWithEntity(IPhysicalEntity entity) { }
        public virtual void OnRemove(World world) { }
        public virtual void Dispose() {}
        public abstract BlockBehaviorSaveData ToSaveData();
    }
}