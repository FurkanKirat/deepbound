using Data.Models.Entities;

namespace Systems.EntitySystem.Interfaces
{
    public interface IEnemy : IActor, IDropsItems, IAttackingEntity
    {
        EnemyData EnemyData { get; }
    }
}