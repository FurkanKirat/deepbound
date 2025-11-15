using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;

namespace Core.Context.Registry
{
    public class NpcStateContext
    {
        public INpc Npc { get; set; }
        public StateMachine<INpc> StateMachine { get; set; }
    }
}