using Core.Context;

namespace GameLoop
{
    public interface ITickable
    {
        void Tick(float timeInterval, TickContext ctx);
    }
}