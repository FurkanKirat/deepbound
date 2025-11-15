using Config;
using Data.Database;
using Data.Models;
using Data.Models.Blocks;
using Systems.EntitySystem;
using Systems.EntitySystem.Interfaces;
using Systems.MovementSystem.Behaviors;
using Systems.Physics.Colliders;
using Systems.StatSystem;
using Systems.WorldSystem;
using UnityEngine;
using Utils;

namespace Systems.MovementSystem
{
    public sealed class EntityMovement
    {
        public IMovementBehavior MovementBehavior { get; set; }
        private readonly CharacterState _characterState;
        private readonly IMovingEntity _entity;
        private StatCollection StatCollection => _entity.StatCollection;
        private readonly World _world;
        
        private Vector2 _knockbackVelocity;
        private float _knockbackTimer;
        private static PhysicsConfig PhysicsConfig => Configs.GameConfig.Physics;
        
        public EntityMovement(IMovementBehavior movementBehavior, IMovingEntity entity, World world)
        {
            _entity = entity;
            _characterState = _entity.CharacterState;
            _world = world;
            MovementBehavior = movementBehavior;
            _entity.CharacterState.IsFacingRight = true;
        }
        
        public void Tick(float deltaTime)
        {
            MovementBehavior.TickMovement(deltaTime, _entity);
            if (_knockbackTimer > 0)
            {
                _knockbackTimer -= deltaTime;
                ApplyKnockbackMovement(deltaTime);
            }

            _entity.CharacterState.IsFacingRight = _entity.Velocity.x switch
            {
                > 0.1f => true,
                < -0.1f => false,
                _ => _entity.CharacterState.IsFacingRight
            };
        }
        
        public void ApplyKnockback(Vector2 force, float duration = 0.25f)
        {
            _knockbackVelocity = force;
            _knockbackTimer = duration;
        }

        public void ApplyGravity(float deltaTime)
        {
            var velocity = _entity.Velocity;
            velocity.y += PhysicsConfig.Gravity * deltaTime;
            if (velocity.y < 0)
                velocity.y = Mathf.Max(velocity.y, PhysicsConfig.MaxFallSpeed);

            if (_characterState.IsGrounded && !_characterState.IsJumping)
                velocity.y = 0;

            SetVelocity(velocity);

            bool moved = TryMove(velocity.y * deltaTime, Axis.Vertical);
            if (!moved && velocity.y <= 0)
            {
                _characterState.IsJumping = false;
            }
        }
        
        public void ApplyHorizontalMovement(float deltaTime, float xInput)
        {
            if (MathUtils.ApproximatelyZero(xInput)) return;

            var velocity = _entity.Velocity;
            velocity.x = xInput * StatCollection.GetStat(StatType.Speed);
            SetVelocity(velocity);

            TryMove(velocity.x * deltaTime, Axis.Horizontal);
        }


        public void Jump()
        {
            var velocity = _entity.Velocity;
            velocity.y = StatCollection.GetStat(StatType.JumpForce);
            SetVelocity(velocity);
            
            _characterState.IsJumping = true;
        }

        public void ApplyFriction()
        {
            var velocity = _entity.Velocity;

            if (_characterState.IsGrounded)
                velocity.x *= PhysicsConfig.GroundFriction;
            else
                velocity.x *= PhysicsConfig.AirFriction;

            SetVelocity(velocity);
        }

        private void SetVelocity(Vector2 newVelocity)
        {
            _entity.Velocity = newVelocity;
        }

        public bool TryMove(Vector2 delta)
        {
            var velocity =  _entity.Velocity;
            WorldPosition estimatedPos = _entity.Position + WorldPosition.FromVector2(delta);
            var estimatedHitbox = new AABBCollider(estimatedPos, _entity.Collider.Bounds.Size);
            foreach (var pos in _world.BlockManager.GetPositionsColliderIn(estimatedHitbox))
            {
                var block = _world.BlockManager.GetBlockAt(pos);
                if (!block.IsSolid()) continue;

                if (_world.BlockManager.IsInsideCollider(pos, estimatedHitbox))
                {
                    velocity.x = delta.x != 0 ? 0 : velocity.x;
                    velocity.y = delta.y != 0 ? 0 : velocity.y;
                    SetVelocity(velocity);
                    return false;
                }
            }

            _entity.Position = estimatedPos;
            SetVelocity(velocity);
            return true;
        }
        private bool TryMove(float delta, Axis axis)
        {
            Vector2 movement = axis == Axis.Horizontal ? new Vector2(delta, 0) : new Vector2(0, delta);
            return TryMove(movement);
        }
        
        private void ApplyKnockbackMovement(float deltaTime)
        {
            var velocity = _knockbackVelocity;
            SetVelocity(velocity);
            
            TryMove(velocity * deltaTime);
            
            _knockbackVelocity.x *= 0.9f;
            _knockbackVelocity.y *= 0.95f;
        }
    }
}
