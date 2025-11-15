using Core.Context;
using Data.Models;
using GameLoop;
using Systems.EntitySystem.Interfaces;
using Systems.WorldSystem;

namespace Systems.EntitySystem.Player
{
    public class MapDiscoverer : ITickable
    {
        private const int Radius = 3;
        
        private readonly World _world;
        private readonly IPlayer _player;

        public MapDiscoverer(World world, IPlayer player)
        {
            _world = world;
            _player = player;
        }

        public void Tick(float timeInterval, TickContext ctx)
        {
            var playerPos = _player.Position.ToTilePosition();
            
            for (int dx = -Radius; dx <= Radius; dx++)
            {
                for (int dy = -Radius; dy <= Radius; dy++)
                {
                    var pos = new TilePosition(playerPos.X + dx, playerPos.Y + dy);
                    _world.CurrentDimension.MinimapDiscovery.Discover(pos);
                }
            }
        }
    }
}