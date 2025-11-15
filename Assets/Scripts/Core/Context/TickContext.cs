using Data.Models;
using GameLoop;
using Systems.EntitySystem.Interfaces;
using Systems.WorldSystem;

namespace Core.Context
{
    public class TickContext
    {
        public PlayerInput PlayerInput { get; set; }
        public IPlayer Player => World.Player;
        public World World { get; set; }
        public GameStateManager GameStateManager { get; set; }
    }
}