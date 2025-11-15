using Data.Models;
using Data.Models.Items;
using Systems.EntitySystem.Interfaces;
using UnityEngine;

namespace Core.Events
{
    public readonly struct PlayerMeleeAttackEvent : IEvent
    {
        public IPlayer User { get; }
        public ItemInstance Item { get; }
        public WorldPosition TargetPosition { get; }
        public float AttackRange { get; }
        public Vector2 Direction { get; }
        
        public PlayerMeleeAttackEvent(
            IPlayer user, 
            ItemInstance item, 
            WorldPosition targetPosition, 
            float attackRange,
            Vector2 direction)
        {
            User = user;
            Item = item;
            TargetPosition = targetPosition;
            AttackRange = attackRange;
            Direction = direction;
        }
    }
}