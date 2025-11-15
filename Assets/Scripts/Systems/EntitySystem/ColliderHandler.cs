using Data.Models;
using Systems.EntitySystem.Interfaces;
using Systems.Physics;
using Systems.Physics.Colliders;
using Systems.WorldSystem;

namespace Systems.EntitySystem
{
    public class ColliderHandler
    {
        public ICollider Collider { get; private set; }
        private readonly WorldPosition _size;
        private readonly World _world;
        private readonly IMovingEntity _entity;
        private WorldPosition _position;

        public WorldPosition Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateCollider();
            }
        }

        public ColliderHandler(IMovingEntity entity, WorldPosition position, WorldPosition size, World world)
        {
            _entity = entity;
            _size = size;
            _world = world;
            _position = position;
            UpdateCollider();
        }
        
        public void UpdateGrounded()
        {
             _entity.CharacterState.IsGrounded = PhysicsUtils.IsGrounded(Collider, _world.BlockManager);
        }
        
        private void UpdateCollider()
        {
            Collider = new AABBCollider(Position, _size);
        }
    }

}