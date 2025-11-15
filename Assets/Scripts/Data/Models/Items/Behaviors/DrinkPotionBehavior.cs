using System.Linq;
using Core.Context;
using Systems.EffectSystem;
using Systems.InventorySystem;

namespace Data.Models.Items.Behaviors
{
    public class DrinkPotionBehavior : IItemBehavior
    {
        public bool TryUse(ItemUseContext context, out string failReason)
        {
            var player = context.User;
            var potion = context.Item;
            
            var potionData = potion.ItemData.PotionData;
            if (potionData == null)
            {
                failReason = "No potion data available";
                return false;
            }

            if (player.HasCooldown(CooldownType.Healing) && potionData.Effects.Any(data => data.Id == EffectIds.Healing))
            {
                failReason = "Has Healing Cooldown";
                return false;
            }
            
            failReason = null;
            return true;
        }


        public void OnSuccess(ItemUseContext context)
        {
            var player = context.User;
            var potion = context.Item;
            
            player.DrinkPotion(potion);
            
            var inv = player.InventoryManager.GetPlayerInventory();
            inv.DecreaseItemCount(
                inv.SelectedSlotIndex,
                1
            );
        }

        public void OnFail(ItemUseContext context, string failReason)
        {
            
        }
    }

}