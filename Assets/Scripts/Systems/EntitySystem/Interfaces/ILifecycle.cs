using System;

namespace Systems.EntitySystem.Interfaces
{
    public interface ILifecycle : IDisposable
    {
        EntityState State { get; }
        void Spawn();
        void Despawn();
    }
    
    public enum EntityState : byte
    {
        Spawned,
        Despawned,
        Disposed
    }
}