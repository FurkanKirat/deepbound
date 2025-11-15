using Core;
using Core.Context;
using Core.Events;
using Data.Database;
using Data.Models.Blocks;
using Systems.InventorySystem;

namespace Utils
{
    public static class PlacementHelper
    {
        public static bool ValidateCommonPlacement(
            string blockId,
            ItemUseContext context,
            out string failReason)
        {
            var user = context.User;
            var item = context.Item;
            
            if (string.IsNullOrEmpty(blockId) || item?.Count <= 0)
            {
                failReason = "Item is not placeable or empty";
                return false;
            }

            var targetTilePos = context.TargetTilePosition;

            if (!RangeHelper.IsInRange(
                    user.Position.ToTilePosition(), 
                    targetTilePos, 
                    user.Config.BlockPlacingRange))
            {
                failReason = "User is not in range";
                return false;
            }

            var blockManager = context.BlockManager;
            if (!blockManager.CanPlaceAt(targetTilePos, blockId))
            {
                var block = blockManager.GetBlockAt(targetTilePos);
                failReason = $"Block {block.Id()} already exists at {targetTilePos}";
                return false;
            }

            var blockData = Databases.Blocks[blockId];
            if (blockData.IsSolid && blockManager.IsInsideCollider(targetTilePos, context.User.Collider))
            {
                failReason = "Cannot place solid block inside player";
                return false;
            }

            failReason = null;
            return true;
        }
        
        public static void PlaceBlock(string blockId, ItemUseContext context)
        {
            var tilePos = context.TargetTilePosition;
            context.BlockManager.SetBlockIfEmpty(tilePos, blockId);
            var block = context.Dimension.BlockManager.GetBlockAt(tilePos);

            var sound = block.GetBlockData().PlaceSound;
            if (sound != null && sound.TryLoad(out var soundData))
                GameEventBus.Publish(new SfxPlayRequest(soundData));
            var player = context.User;
            var inv = player.InventoryManager.GetPlayerInventory();
            inv.DecreaseItemCount(
                inv.SelectedSlotIndex,
                1
            );
        }
    }
}