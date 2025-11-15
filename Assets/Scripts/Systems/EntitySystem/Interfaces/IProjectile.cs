using Core.Context;
using Data.Models.Entities;

namespace Systems.EntitySystem.Interfaces
{
    public interface IProjectile : 
        IMovingEntity
    {
        ProjectileData ProjectileData { get; }
        IPhysicalEntity Owner { get; }
        DamageContext Damage { get; }
    }

}