using Data.Models;
using Systems.EntitySystem.Interfaces;

namespace Core.Context.Registry
{
    public class MovementBehaviorContext
    {
        public IPhysicalEntity SpawnerEntity { get; set; }
        public IMovingEntity BehaviorOwner { get; set; }
        public WorldPosition TargetPos { get; set; }
    }
}