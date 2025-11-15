using Core.Context;
using Utils;
using Utils.Extensions;

namespace Data.Models.Items.Behaviors
{
    public class PlaceBlockBehavior : IItemBehavior
    {
        public bool TryUse(ItemUseContext context, out string failReason)
        {
            var blockId = context.Item.GetBlockId();
            return PlacementHelper.ValidateCommonPlacement(blockId, context, out failReason);
        }


        public void OnSuccess(ItemUseContext context)
        {
            var blockId = context.Item.GetBlockId();
            PlacementHelper.PlaceBlock(blockId, context);
        }

        public void OnFail(ItemUseContext context, string failReason)
        {
            
        }
    }

}