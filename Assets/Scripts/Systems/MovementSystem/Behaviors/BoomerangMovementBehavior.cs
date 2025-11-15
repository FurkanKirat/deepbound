using Core;
using Core.Events;
using Data.Models;
using Systems.EntitySystem.Interfaces;
using Systems.StatSystem;
using Utils;

namespace Systems.MovementSystem.Behaviors
{
    public class BoomerangMovementBehavior : IMovementBehavior
    {
        private readonly IPhysicalEntity _boomerangOwner;

        private readonly WorldPosition _targetPosition;

        private readonly float _speed;
        
        private bool _returning = false;

        public BoomerangMovementBehavior(IPhysicalEntity boomerangOwner, IMovingEntity behaviorOwner, WorldPosition targetPosition)
        {
            _boomerangOwner = boomerangOwner; 
            _speed = behaviorOwner.StatCollection.GetStat(StatType.Speed);
            var range = behaviorOwner.StatCollection.GetStat(StatType.Range);
            var startPosition = boomerangOwner.Position;
            var dir = targetPosition - startPosition;
            var dirNormalized = dir.Normalized;
            
            _targetPosition = startPosition + range * dirNormalized;
            GameLogger.Log($"Start: {startPosition}, Target: {targetPosition}, DirNormalized: {dirNormalized}, CalculatedTarget: {_targetPosition}");

        }
        public void TickMovement(float deltaTime, IMovingEntity owner)
        {
            var currentPos = owner.Position;
            var target = _returning ? 
                _boomerangOwner.Position : 
                _targetPosition;
            
            var direction = (target - currentPos).Normalized;
            var movement = direction * _speed * deltaTime;

            float sqrDistanceToTarget = (target - currentPos).SqrMagnitude;
            float sqrMovementDistance = movement.SqrMagnitude;

            if (sqrDistanceToTarget <= sqrMovementDistance || sqrMovementDistance <= 0.0001)
            {
                if (!_returning)
                {
                    _returning = true;
                }
                else
                {
                    GameEventBus.Publish(new EntityDestroyRequest(owner));
                }
                return;
            }
            bool moved = owner.Movement.TryMove(movement.ToVector2());

            if (!moved && !_returning)
            {
                _returning = true;
            }
        }

    }
}