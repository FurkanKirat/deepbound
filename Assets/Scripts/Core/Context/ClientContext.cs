using Systems.EntitySystem.Interfaces;
using Systems.WorldSystem;
using Visuals.CameraScripts;

namespace Core.Context
{
    public class ClientContext
    {
        public World World { get; set; }
        public CameraManager CameraManager { get; set; }
        public IPlayer Player => World.Player;
        public Dimension Dimension => World.CurrentDimension;
        public BlockManager BlockManager => World.BlockManager;
    }

}