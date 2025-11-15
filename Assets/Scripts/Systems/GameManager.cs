using System;
using Core.Context;
using GameLoop;
using Interfaces;
using Systems.InputSystem;
using Systems.SaveSystem;
using Systems.WorldSystem;

namespace Systems
{
    public class GameManager : ITickable, IDisposable, IInitializable
    {
        public World World { get; }
        public GameSaver GameSaver { get; }
        public TickManager TickManager { get; }
        public GameStateManager GameStateManager { get; }
        
        public GameManager(World world)
        {
            World = world;
            GameStateManager = new GameStateManager();
            GameSaver = new GameSaver(this);
            TickManager = new TickManager();
            TickManager.Register(this);
        }

        public void Tick(float timeInterval, TickContext ctx)
        {
            ctx.PlayerInput = InputCache.Current;
            World.Tick(timeInterval, ctx);
        }

        public void Initialize()
        {
            World.Initialize();
            TickManager.Start();
        }

        public void Dispose()
        {
            GameSaver.Dispose();
            World.Dispose();
            GameSaver.Dispose();
        }
    }
}