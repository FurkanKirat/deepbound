using Systems.EntitySystem.Interfaces;

namespace Visuals.ObjectVisuals
{
    public class ItemEntityVisual : BaseEntityVisual<IItemEntity>
    {
        public override void OnSpawn()
        {
            Renderer.sprite = Data.ItemInstance.
                ItemData.Icon.Load();
            base.OnSpawn();
        }
    }
}