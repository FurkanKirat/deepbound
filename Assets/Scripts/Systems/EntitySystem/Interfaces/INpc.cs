using Data.Models.Entities;

namespace Systems.EntitySystem.Interfaces
{
    public interface INpc : IActor
    {
        public NpcData NpcData { get; }
    }
}