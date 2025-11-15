using Core;
using Core.Context.Registry;
using Core.Events;
using Generated.Ids;
using Systems.EntitySystem;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Utils;

namespace Data.Models.Blocks.Behaviors
{
    public class PortalBehavior : BaseBlockBehavior
    {
        public override string BehaviorId => BlockBehaviorIds.Portal;

        public string DimensionId { get; set; }
        public PortalBehavior(BlockBehaviorContext ctx) : base(ctx.Position)
        {
            DimensionId = ctx.DimensionId;
        }

        public PortalBehavior(BlockBehaviorContext ctx, PortalBehaviorSaveData portal) : base(ctx.Position)
        {
            DimensionId = portal.DimensionId;
        }
        
        public override void OnCollisionWithEntity(IPhysicalEntity entity)
        {
            if (entity.Type != EntityType.Player || DimensionId == null) return;
            GameLogger.Log($"Changing dimension to {DimensionId}",nameof(PortalBehavior));
            GameEventBus.Publish(new DimensionChangeRequest(DimensionId));
        }

        public override BlockBehaviorSaveData ToSaveData()
        {
            return new PortalBehaviorSaveData
            {
                BehaviorId = BehaviorId,
                DimensionId = DimensionId
            };
        }
        
    }
}