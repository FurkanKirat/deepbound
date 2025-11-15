using System;
using Core;
using Core.Context;
using Core.Events;
using Data.Models;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using Systems.WorldSystem;
using UnityEngine;
using Utils;

namespace Systems.EntitySystem.Player
{
    public class PlayerItemUseHandler : IDisposable
    {
        private readonly IPlayer _player;
        private readonly World _world;

        private float _lastUseTime;

        public PlayerItemUseHandler(World world, IPlayer player)
        {
            _world = world;
            _player = player;
            
            GameEventBus.Subscribe<PrimaryUseStarted>(OnPrimaryUseStarted);
            GameEventBus.Subscribe<PrimaryUseHeld>(OnPrimaryUseHeld);
            GameEventBus.Subscribe<PrimaryUseEnded>(OnPrimaryUseEnded);
            GameEventBus.Subscribe<PrimaryUseClicked>(OnPrimaryUseClicked);
        }


        public void Dispose()
        {
            GameEventBus.Unsubscribe<PrimaryUseStarted>(OnPrimaryUseStarted);
            GameEventBus.Unsubscribe<PrimaryUseHeld>(OnPrimaryUseHeld);
            GameEventBus.Unsubscribe<PrimaryUseEnded>(OnPrimaryUseEnded);
            GameEventBus.Unsubscribe<PrimaryUseClicked>(OnPrimaryUseClicked);
        }
        
        private void OnPrimaryUseClicked(PrimaryUseClicked e)
        {
            UseItem(e.ScreenPosition, e.WorldPosition);
        }

        private void OnPrimaryUseHeld(PrimaryUseHeld e)
        {
        }

        private void OnPrimaryUseStarted(PrimaryUseStarted e)
        {
            UseItem(e.ScreenPosition, e.WorldPosition);
        }

        private void OnPrimaryUseEnded(PrimaryUseEnded e)
        {
        }
        
        private void UseItem(Vector2 screenPos, WorldPosition worldPos)
        {
            var item = _player.InventoryManager.GetPlayerInventory().GetSelectedItem();
            if (item == null || item.IsEmpty) return;
            
            float useTime = item.ItemData.UseTime;
            
            if (Time.time < _lastUseTime + useTime)
            {
                return;
            }
            
            _lastUseTime = Time.time;
            
            var behaviorRef = item.ItemData.Behavior;
            var behavior = behaviorRef?.Load();
            if (behaviorRef == null || behavior == null) return;
            
            var context = new ItemUseContext
            {
                User = _player,
                Item = item,
                World = _world,
                TargetPosition = worldPos,
                TargetTilePosition = worldPos.ToTilePosition(),
                ScreenPosition = screenPos,
            };

            var posX = (worldPos - _player.Position).x;
            _player.CharacterState.IsFacingRight = posX switch
            {
                > 0.1f => true,
                < -0.1f => false,
                _ => _player.CharacterState.IsFacingRight
            };

            bool success = behavior.TryUse(context, out var failReason);
            GameLogger.Log($"TryUse success: {success}, failReason: {failReason}");

            if (success)
            {
                behavior.OnSuccess(context);
            }
            else
            {
                behavior.OnFail(context, failReason);
            }
        }

    }

}