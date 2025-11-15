using System;
using Core;
using Core.Events;
using Data.Models.Blocks;
using Systems.EntitySystem.Interfaces;
using Systems.WorldSystem;

namespace Systems.EntitySystem.Player
{
    public class PlayerInteractionHandler : IDisposable
    {
        private readonly IPlayer _player;
        private readonly World _world;

        public PlayerInteractionHandler(World world, IPlayer player)
        {
            _world = world;
            _player = player;

            GameEventBus.Subscribe<SecondaryUseRequested>(OnSecondaryUseStarted);
        }

        public void Dispose()
        {
            GameEventBus.Unsubscribe<SecondaryUseRequested>(OnSecondaryUseStarted);
        }

        private void OnSecondaryUseStarted(SecondaryUseRequested e)
        {
            var pos = e.WorldPosition;
            var tilePos = pos.ToTilePosition();
            
            if (_world.BlockManager.TryGetInteractable(pos, out var interactable))
            {
                interactable.Interact(_player, _world);
                var blockData = _world.BlockManager.GetBlockAt(tilePos).GetBlockData();
                
                if (blockData.InteractSound?.TryLoad(out var sound) == true)
                    GameEventBus.Publish(new SfxPlayRequest(sound));
            }
        }
    }

}