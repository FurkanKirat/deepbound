using System;

namespace Systems.StartingSystem.GameManagers
{
    public interface IGameManagerProvider
    {
        void RequestGameManager(Action<GameManager> onReady);
    }
}