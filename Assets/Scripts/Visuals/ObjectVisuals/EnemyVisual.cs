using Systems.EntitySystem.Interfaces;

namespace Visuals.ObjectVisuals
{
    public class EnemyVisual : BaseEntityVisual<IEnemy>
    {
        public override void OnSpawn()
        {
            Renderer.sprite = Data.EnemyData.Icon.Load();
            base.OnSpawn();
        }
    }
}