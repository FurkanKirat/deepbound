using Core;
using Core.Context.Registry;
using Core.Events;
using Generated.Ids;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Systems.WorldSystem;
using Utils;

namespace Data.Models.Blocks.Behaviors
{
    public class BedBehavior : BaseBlockBehavior
    {
        public override string BehaviorId => BlockBehaviorIds.Bed;

        public BedBehavior(BlockBehaviorContext context) : base(context.Position)
        {
            
        }
        public override void Interact(IPlayer player, World world)
        {
            GameLogger.Log($"Changing player spawn to {Position} ",nameof(BedBehavior));
            GameEventBus.Publish(new PlayerSpawnChangedEvent(Position.ToWorldPosition()));
        }
        
        public override BlockBehaviorSaveData ToSaveData()
        {
            return new BlockBehaviorSaveData
            {
                BehaviorId = BehaviorId,
            };
        }
        
    }
}