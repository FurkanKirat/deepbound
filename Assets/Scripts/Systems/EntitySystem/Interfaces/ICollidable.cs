namespace Systems.EntitySystem.Interfaces
{
    public interface ICollidable
    {
        void OnCollisionWithEntity(IPhysicalEntity entity);
    }
}