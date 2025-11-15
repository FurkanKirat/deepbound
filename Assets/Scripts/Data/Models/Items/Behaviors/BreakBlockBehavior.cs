using Constants;
using Core;
using Core.Context;
using Core.Events;
using Data.Models.Items.SubData;
using Generated.Tags;
using Utils;
using Utils.Extensions;

namespace Data.Models.Items.Behaviors
{
    public class BreakBlockBehavior : IItemBehavior
    {
        public bool TryUse(ItemUseContext context, out string failReason)
        {
            var user = context.User;
            var item = context.Item;

            if (item.GetToolType() != ToolType.Pickaxe || !item.HasTag(ItemTags.Pickaxe))
            {
                failReason = "You need a pickaxe to mine this block.";
                return false;
            }

            var targetTilePos = context.TargetTilePosition;

            if (!RangeHelper.IsInRange(user.Position.ToTilePosition(), targetTilePos, user.Config.BlockBreakingRange))
            {
                failReason = "User is not in range";
                return false;
            }
            
            if (!context.BlockManager.CanBreakAt(targetTilePos))
            {
                failReason = "Block is not breakable.";
                return false;
            }
            failReason = null;
            return true;
        }

        public void OnSuccess(ItemUseContext context)
        {
            var player = context.User;
            var tilePos = context.TargetTilePosition;
            var block = context.BlockManager.GetBlockAt(tilePos);
            GameEventBus.Publish(new BlockBreakRequest(block, tilePos, context.Item, player));
        }

        public void OnFail(ItemUseContext context, string failReason)
        {
            
        }
    }
}