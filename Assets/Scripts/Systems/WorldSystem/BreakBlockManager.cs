using System;
using Core;
using Core.Context;
using Core.Events;
using Data.Models;
using Data.Models.Blocks;
using Data.Models.Blocks.Behaviors;
using Data.Models.Items;
using GameLoop;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using Utils;

namespace Systems.WorldSystem
{
    public class BreakBlockManager : ITickable, IDisposable
    {
        private readonly World _world;
        
        private float _remainingBreakTime;
        private float _totalBreakTime;
        private bool _isBreaking;
        
        private Block _currentBlock;
        private TilePosition _currentBlockPosition;
        private BlockData _currentBlockData;
        private BlockEntity _currentBlockEntity;
        
        public BreakBlockManager(World world)
        {
            _world = world;
            GameEventBus.Subscribe<BlockBreakRequest>(OnBlockBreakRequest);
            GameEventBus.Subscribe<PrimaryUseEnded>(OnPrimaryUseEnded);
            GameEventBus.Subscribe<DimensionChangedEvent>(OnDimensionChanged);
        }

        public void Dispose()
        {
            GameEventBus.Unsubscribe<BlockBreakRequest>(OnBlockBreakRequest);
            GameEventBus.Unsubscribe<PrimaryUseEnded>(OnPrimaryUseEnded);
            GameEventBus.Unsubscribe<DimensionChangedEvent>(OnDimensionChanged);
        }
        
        private void OnDimensionChanged(DimensionChangedEvent e)
        {
            StopBreaking();
            _currentBlock = default;
            _currentBlockData = null;
            _currentBlockEntity = null;
            _currentBlockPosition = default;
        }

        
        public void Tick(float timeInterval, TickContext ctx)
        {
            if (!_isBreaking) return;
            _remainingBreakTime -= timeInterval;
            
            float progress = 1f - (_remainingBreakTime / _totalBreakTime);
            progress = Math.Clamp(progress, 0f, 1f);

            GameEventBus.Publish(new BlockBreakingProgress(_currentBlockPosition, _currentBlock, progress));
            
            if (_remainingBreakTime <= 0)
                BlockBroken(ctx.Player);
                
        }
        
        // Logic methods
        private void StopBreaking()
        {
            if (_isBreaking)
            {
                GameLogger.Log("[BreakBlockManager] Breaking stopped by player.");
                _isBreaking = false;
                GameEventBus.Publish(new BlockBreakCancelledEvent(_currentBlockPosition));
            }
        }
        
        private void BlockBroken(IPlayer player)
        {
            _isBreaking = false;
            _world.BlockManager.BreakBlock(_currentBlockPosition);

            var sound = _currentBlockData.BreakSound;
            if (sound != null && sound.TryLoad(out var audioClip))
                GameEventBus.Publish(new SfxPlayRequest(audioClip));
            var itemAmount = _currentBlockData.DropItem;
            if (_currentBlockEntity != null && _currentBlockEntity.TryGetBehavior<CropBehavior>(out var behavior))
            {
                if (behavior.GrowthStage == GrowthStage.Mature)
                    itemAmount = _currentBlockData.Crop.HarvestItem;
                else
                    itemAmount = null;
            }
            
            if (itemAmount != null)
                player.InventoryManager.GetPlayerInventory().AcceptItem(itemAmount.ToItemInstance());
            
        }

        private void StartBreaking(ItemInstance item, Block block, TilePosition blockPos)
        {
            if (_isBreaking) return;
            
            _isBreaking = true;
            _currentBlock = block;
            _currentBlockPosition = blockPos;
            _currentBlockData = _currentBlock.GetBlockData();
            _currentBlockEntity = _world.BlockManager.GetTileEntityAt(_currentBlockPosition);
            _remainingBreakTime = _totalBreakTime = CalculateBreakingTime(item);
        }

        private float CalculateBreakingTime(ItemInstance item)
        {
            var hardness = _currentBlockData.Hardness;
            var speed = item.GetToolSpeed();
            
            return hardness / speed;
        }
        
        // Event Methods
        private void OnBlockBreakRequest(BlockBreakRequest e)
        {
            GameLogger.Log("OnBlockBreakRequest called for breaking block!");
            StartBreaking(e.ItemInstance, e.Block, e.BlockPosition);
        }
        private void OnPrimaryUseEnded(PrimaryUseEnded e)
        {
            StopBreaking();
        }
    }
}