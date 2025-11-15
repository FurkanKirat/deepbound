using Core;
using Core.Context;
using Core.Context.Spawn;
using Core.Events;
using Generated.Ids;
using Generated.Tags;
using Systems.EntitySystem.Projectile;
using Systems.InventorySystem;
using Systems.Physics;
using Systems.StatSystem;

namespace Data.Models.Items.Behaviors
{
    public class ShootArrowBehavior : IItemBehavior
    {
        public bool TryUse(ItemUseContext context, out string failReason)
        {
            var user = context.User;
            
            if (!user.InventoryManager.GetPlayerInventory().HasItemWithTag(ItemTags.Arrow))
            {
                failReason = "No arrow found";
                return false;
            }
            
            failReason = null;
            return true;
        }

        public void OnSuccess(ItemUseContext context)
        {
            var user = context.User;
            var item = context.Item;
            var userPos = user.Position;
            var targetPos = context.TargetPosition;
            
            float damage = item.GetDamage() * user.StatCollection.GetStat(StatType.DamageMultiplier);

            WorldPosition handOffset = user.CharacterState.IsFacingRight
                ? user.Config.HandOffset
                : user.Config.HandOffset.XNegated;
            var ctx = new ProjectileSpawnContext
            {
                SubTypeId = ProjectileIds.Arrow,
                Owner = user,
                SpawnPosition = userPos + handOffset,
                Direction = PhysicsUtils.GetDirectionToCursor(userPos, targetPos),
                World = context.World,
                DamageContext = new DamageContext(
                    damage,
                    user.StatCollection.GetStat(StatType.CritRate),
                    user.StatCollection.GetStat(StatType.CritDamage))
            };
            var arrowProjectile = new AttackingProjectileLogic(ctx);
            GameEventBus.Publish(new EntitySpawnRequest(arrowProjectile));
            context.User.InventoryManager.GetPlayerInventory().RemoveItemWithTag(ItemTags.Arrow, 1);
        }

        public void OnFail(ItemUseContext context, string failReason)
        {
            
        }
    }
}