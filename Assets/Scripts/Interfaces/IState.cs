using Core.Context;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.State;

namespace Interfaces
{
    public interface IState<T> : ISaveable<StateSaveData>
    {
        string Id { get; }
        void Enter();
        void Exit();
        void Tick(float deltaTime, TickContext ctx);
    }

}