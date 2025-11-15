using System;
using Systems.EntitySystem.Interfaces;

namespace Core.Context
{
    public class AttackContext
    {
        public IAttackingEntity AttackingEntity { get; set; }
        public ITargetEntity TargetEntity { get; set; }
        public Func<ITargetEntity, bool> TargetFilter { get; set; }
        public Random Random { get; set; }
        
        public DamageContext DamageContext { get; set; }
    }
}