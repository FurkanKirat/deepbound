using Core;
using Core.Context;
using Core.Events;
using Systems.CombatSystem.Damage;
using Systems.CombatSystem.Interfaces;
using Systems.EntitySystem;
using Systems.Physics;
using Systems.StatSystem;
using UnityEngine;

namespace Data.Models.Items.Behaviors
{
    public class ItemMeleeAttackBehavior : IItemBehavior
    {
        public bool TryUse(ItemUseContext context, out string failReason)
        {
            var item = context.Item;

            if (!item.IsWeapon())
            {
                failReason = "This item cannot be used as a weapon.";
                return false;
            }

            failReason = null;
            return true;
        }

        public void OnSuccess(ItemUseContext context)
        {
            var player = context.User;
            var item = context.Item;
            float attackRange = item.GetWeaponRange();
            float damage = item.GetDamage() * player.StatCollection.GetStat(StatType.DamageMultiplier);
            
            Vector2 forward = (context.TargetPosition - player.Position).ToVector2().normalized;
            
            var nearbyEntities = context.EntityManager
                .GetEntitiesWithinRadius(player.Position, attackRange);

            foreach (var target in nearbyEntities)
            {
                if (target.Type == EntityType.Player)
                    continue;
                
                var dir = target.Position - player.Position;
                float distanceSquared = dir.SqrMagnitude;
                
                if (distanceSquared < attackRange * attackRange)
                {
                    var dirNormalized = (dir / Mathf.Sqrt(distanceSquared)).ToVector2();
                    float dot = Vector2.Dot(forward, dirNormalized);
                    
                    if (dot > 0.7f && target is IDamageable damageable)
                    {
                        var knockback = PhysicsUtils.GetKnockback(player.Position, target.Position);
                        var damageInfo = new DamageInfo(
                            damage, 
                            item.GetDamageType(), 
                            player,
                            knockback,
                            context.Random,
                            player.StatCollection.GetStat(StatType.CritRate),
                            player.StatCollection.GetStat(StatType.CritDamage));
                        damageable.TakeDamage(damageInfo);
                    }
                }
            }

            GameEventBus.Publish(
                new PlayerMeleeAttackEvent(player, item, context.TargetPosition, attackRange, forward));
        }

        public void OnFail(ItemUseContext context, string failReason)
        {
            
        }
    }
}