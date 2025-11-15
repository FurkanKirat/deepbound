using Core;
using Core.Context.Registry;
using Core.Events;
using Data.Models.Crafting;
using Generated.Ids;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Systems.WorldSystem;

namespace Data.Models.Blocks.Behaviors
{
    public class CraftingBehavior : BaseBlockBehavior
    {
        public override string BehaviorId => BlockBehaviorIds.Crafting;
        private readonly CraftingStation _station;

        public CraftingBehavior(BlockBehaviorContext ctx) : base(ctx.Position)
        {
            _station = ctx.BlockData.Station.Station;
        }
        
        public override void Interact(IPlayer player, World world)
        {
            GameEventBus.Publish(new InventoryOpenRequestEvent(new InventoryOpenConfig(player, new []
                {
                    SlotCollectionType.Inventory
                }), _station));
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