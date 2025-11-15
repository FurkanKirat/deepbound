using Data.Models.Items.Behaviors;
using Data.RegistrySystem;
using Generated.Ids;

namespace Data.Registrars
{
    public class ItemBehaviorRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            Registries.ItemBehaviors.RegisterMany(
                (ItemBehaviorIds.PlaceBlock, new PlaceBlockBehavior()),
                (ItemBehaviorIds.MineBlock, new BreakBlockBehavior()),
                (ItemBehaviorIds.MeleeAttack, new ItemMeleeAttackBehavior()),
                (ItemBehaviorIds.ShootArrow, new ShootArrowBehavior()),
                (ItemBehaviorIds.ThrowBoomerang, new BoomerangItemBehavior()),
                (ItemBehaviorIds.Seed, new SeedItemBehavior()),
                (ItemBehaviorIds.DrinkPotion, new DrinkPotionBehavior())
            );
        }
    }
}