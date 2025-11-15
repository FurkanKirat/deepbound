using Core;
using Core.Context;
using Core.Context.Spawn;
using Core.Events;
using Generated.Ids;
using Systems.EntitySystem.Projectile;
using Systems.Physics;
using Systems.StatSystem;

namespace Data.Models.Items.Behaviors
{
    public class BoomerangItemBehavior : IItemBehavior
    {
        public bool TryUse(ItemUseContext context, out string failReason)
        {
            failReason = null;
            return true;
        }

        public void OnSuccess(ItemUseContext context)
        {
            var user = context.User;
            var item = context.Item;
            var userPos = user.Position;
            var targetPos = context.TargetPosition;
            var statCollection = user.StatCollection;
            var ctx = new ProjectileSpawnContext
            {
                SubTypeId = ProjectileIds.Pillow,
                Owner = user,
                SpawnPosition = userPos + new WorldPosition(0.5f, 0.5f),
                TargetPosition = targetPos,
                Direction = PhysicsUtils.GetDirectionToCursor(userPos, targetPos),
                World = context.World,
                DamageContext = new DamageContext(
                    item.GetDamage() * statCollection.GetStatOrDefault(StatType.DamageMultiplier, 1f),
                    statCollection.GetStat(StatType.CritRate),
                    statCollection.GetStat(StatType.CritDamage))
            };
            var boomerangProjectile = new AttackingProjectileLogic(ctx);
            GameEventBus.Publish(new EntitySpawnRequest(boomerangProjectile));
        }

        public void OnFail(ItemUseContext context, string failReason)
        {
            
        }
    }
}