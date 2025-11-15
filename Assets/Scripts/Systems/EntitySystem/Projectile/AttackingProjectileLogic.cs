using Core.Context;
using Core.Context.Registry;
using Core.Context.Spawn;
using Systems.CombatSystem.Behaviors;
using Systems.EntitySystem.Interfaces;

namespace Systems.EntitySystem.Projectile
{
    public class AttackingProjectileLogic : ProjectileLogic, IAttackingEntity
    {
        public IAttackBehavior AttackBehavior { get; private set; }

        public AttackingProjectileLogic(ProjectileSpawnContext spawnContext) : base(spawnContext)
        {
            AttackBehavior = ProjectileData.AttackBehavior.Create(new AttackBehaviorContext());
        }

        public override void OnCollisionWithEntity(IPhysicalEntity target)
        {
            base.OnCollisionWithEntity(target);

            if (target is not ITargetEntity targetEntity) 
                return;
            var attackContext = new AttackContext
            {
                AttackingEntity = this,
                TargetEntity = targetEntity,
                TargetFilter = entity => !entity.IsDead && Owner.Type != entity.Type,
                Random = World.Random,
                
                DamageContext = Damage
            };
        
            this.AttackIfNeeded(attackContext);
        }
    }
}