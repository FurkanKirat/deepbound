using Core.Context;
using Utils;
using Utils.Extensions;

namespace Data.Models.Items.Behaviors
{
    public class SeedItemBehavior : IItemBehavior
    {
        public bool TryUse(ItemUseContext context, out string failReason)
        {
            var item = context.Item;
            var blockId = item.CropId();
            if(!PlacementHelper.ValidateCommonPlacement(blockId, context, out failReason))
                return false;
            
            if (!context.BlockManager.CanPlaceCropAt(context.TargetTilePosition, blockId))
            {
                failReason = "Below block is not dirt";
                return false;
            }
            return true;
        }


        public void OnSuccess(ItemUseContext context)
        {
            var cropId = context.Item.CropId();
            PlacementHelper.PlaceBlock(cropId, context);
        }

        public void OnFail(ItemUseContext context, string failReason)
        {
            
        }
    }
}