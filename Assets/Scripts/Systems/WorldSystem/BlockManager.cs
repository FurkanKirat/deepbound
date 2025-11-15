using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Context;
using Core.Events;
using Data.Database;
using Data.Models;
using Data.Models.Blocks;
using Data.Models.Blocks.Behaviors;
using GameLoop;
using Generated.Ids;
using Systems.Physics.Colliders;
using Systems.SaveSystem.SaveData;
using Utils;

namespace Systems.WorldSystem
{
    public class BlockManager : 
        ITickable,
        IDisposable
    {
        
        private readonly World _world;
        #region Properties

        private readonly WorldGrid<Block> _blocks;
        private readonly Dictionary<TilePosition,BlockEntity> _blockEntities = new();
        public int Width => _blocks.Width;
        public int Height => _blocks.Height;
        
        public IEnumerable<BlockEntity> BlockEntities => _blockEntities.Values;
        public IReadOnlyDictionary<TilePosition, BlockEntity> BlockEntitiesByPosition => _blockEntities;
        #endregion
        
        #region Constructors & Dispose
        public BlockManager(WorldGrid<Block> blocks, World world)
        {
            _blocks = blocks;
            _world = world;
            for(int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                var blockData = blocks[x, y].GetBlockData();
                if (_blocks[x,y].IsMaster && (blockData.HasAnyBehavior || blockData.IsMultiple))
                {
                    var blockEntity = BlockEntity.Create(new TilePosition(x, y), blockData.Id, true);
                    AddBlockEntity(blockEntity);
                }
            }
        }
        
        public BlockManager(BlocksSaveData saveData, World world)
        {
            _world = world;
            foreach (var blockSave in saveData.BlockEntities)
            {
                var masterPos = blockSave.Position;
                _blockEntities[masterPos] = BlockEntity.Load(blockSave);
            }
            var width = saveData.WorldGrid.Width;
            var height = saveData.WorldGrid.Height;
            _blocks = new WorldGrid<Block>(width, height);
            
            for(int y = 0; y < height; y++) 
            for (int x = 0; x < width; x++)
            {
                var saveId = saveData.WorldGrid[x, y];
                var pos = new TilePosition(x, y);
                if (_blockEntities.TryGetValue(pos, out BlockEntity blockEntity))
                {
                    for(byte offsetX = 0; offsetX < blockEntity.Width; offsetX++)
                    for (byte offsetY = 0; offsetY < blockEntity.Height; offsetY++)
                    {
                        _blocks[x + offsetX, y + offsetY] = offsetX == 0 && offsetY == 0 ?
                            Block.CreateMaster(saveId) :
                            Block.CreateSlave(saveId, offsetX, offsetY);
                    }
                }
                else if(_blocks[x,y].IsAir())
                {
                    _blocks[x,y] = Block.CreateMaster(saveId);
                }
                
            }
            
        }

        public void Dispose()
        {
            foreach (var (_,blockEntity) in _blockEntities)
            {
                blockEntity.Dispose();
            }
            
            _blocks.Clear();
            _blockEntities.Clear();
        }

        #endregion
        public BlocksSaveData ToSaveData()
        {
            var worldGrid = new WorldGrid<ushort>(Width, Height);
            var blockEntities = new List<BlockEntitySaveData>();
            
            for(int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                ushort numId = _blocks[x, y].BlockType;
                worldGrid[x, y] = numId;
            }

            foreach (var (_, tileEntity) in _blockEntities)
            {
                blockEntities.Add(tileEntity.ToSaveData());
            }

            return new BlocksSaveData
            {
                WorldGrid = worldGrid,
                BlockEntities = blockEntities
            };
        }
        
        #region Block Access
        public bool IsInsideBounds(TilePosition tilePosition) =>
            IsInsideBounds(tilePosition.X, tilePosition.Y);
        
        public bool IsInsideBounds(int x, int y) =>
            _blocks.IsInBounds(x,y);
        
        public BlockEntity GetTileEntityAt(TilePosition pos)
        {
            var block = GetBlockAt(pos);
            var masterPos = new TilePosition(pos.X - block.OffsetX, pos.Y - block.OffsetY);
            return _blockEntities.GetValueOrDefault(masterPos, null);
        }
             
        public Block GetBlockAt(TilePosition pos)
            => GetBlockAt(pos.X, pos.Y);
        
        public Block GetBlockAt(int x, int y)
            => IsInsideBounds(x,y) ? _blocks[x, y] : default;

        public Block GetBlockBelow(TilePosition pos)
        {
            var belowPos = new TilePosition(pos.X, pos.Y - 1);
            return GetBlockAt(belowPos);
        }
        
        public Block GetBlockAbove(TilePosition pos)
        {
            var belowPos = new TilePosition(pos.X, pos.Y + 1);
            return GetBlockAt(belowPos);
        }

        public Block GetBlockRight(TilePosition pos)
        {
            var belowPos = new TilePosition(pos.X + 1, pos.Y);
            return GetBlockAt(belowPos);
        }

        public Block GetBlockLeft(TilePosition pos)
        {
            var belowPos = new TilePosition(pos.X - 1, pos.Y);
            return GetBlockAt(belowPos);
        }

        public bool IsSolidAt(TilePosition tilePosition) =>
            !IsInsideBounds(tilePosition) || GetBlockAt(tilePosition).IsSolid();
        
        public string GetTextureId(TilePosition pos)
        {
            var block = GetBlockAt(pos);
            var blockEntity = GetTileEntityAt(pos);
            var blockData = block.GetBlockData();
            string baseId = blockData.Id;

            if (blockEntity != null && blockEntity.TryGetBehavior<CropBehavior>(out var crop))
            {
                return $"{baseId}_{crop.GrowthStage.ToIntSafe()}";
            }

            return blockData.IdleAnimation.Frames[0].Key;
        }
        #endregion
        
        #region Block Modification
        public bool PlaceBlockAt(TilePosition tilePosition, string blockId)
        {
            if (!IsInsideBounds(tilePosition)) return false;
            
            var blockData = Databases.Blocks[blockId];
            var blockSize = blockData.Size;
            
            var blocks = new List<TilePosition>();
                
            for (byte y = 0; y < blockSize.y; y++)
            for (byte x = 0; x < blockSize.x; x++)
            {
                var pos = tilePosition + new TilePosition(x, y);
                if (!IsInsideBounds(pos)) continue;
                var block = (x == 0 && y == 0)
                    ? Block.CreateMaster(blockId)
                    : Block.CreateSlave(blockId, x , y);

                _blocks[pos.X, pos.Y] = block;
                blocks.Add(pos);
            }
            
            _blockEntities.Remove(tilePosition);
            if (blockData.IsMultiple || blockData.HasAnyBehavior)
            {
                _blockEntities[tilePosition] = BlockEntity.Create(tilePosition, blockId);
            }

            GameEventBus.Publish(new BlockPlacedEvent(blocks));
            return true;
        }
        
        public bool SetBlockIfEmpty(TilePosition tilePosition, string blockId)
        {
            if (!CanPlaceAt(tilePosition, blockId)) return false;
            PlaceBlockAt(tilePosition, blockId);
            return true;
        }

        public void BreakBlock(TilePosition tilePosition)
        {
            var block = GetBlockAt(tilePosition);
            if (block.IsAir()) return;

            var masterPos = new TilePosition(tilePosition.X - block.OffsetX, tilePosition.Y - block.OffsetY);
            if (_blockEntities.TryGetValue(masterPos, out var masterEntity))
            {
                var width = masterEntity.Width;
                var height = masterEntity.Height;

                var blocks = new List<TilePosition>();
                for (int dx = 0; dx < width; dx++)
                for (int dy = 0; dy < height; dy++)
                {
                    var pos = new TilePosition(masterPos.X + dx, masterPos.Y + dy);
                    if (!IsInsideBounds(pos)) continue;
                    _blocks[pos.X, pos.Y] = Block.CreateMaster(BlockIds.Air);
                    blocks.Add(pos);
                }
            
                _blockEntities.Remove(masterEntity.Position);
                masterEntity.OnRemove(_world);
                GameEventBus.Publish(new BlockDestroyedEvent(blocks));
            }
            else
            {
                _blocks[tilePosition.X, tilePosition.Y] = Block.CreateMaster(BlockIds.Air);
                GameEventBus.Publish(new BlockDestroyedEvent(tilePosition));
            }
        }
        #endregion
        
        #region Block Rules
        public bool CanPlaceAt(TilePosition tilePosition, string blockId)
        {
            var blockData = Databases.Blocks[blockId];
            var blockSize = blockData.Size;

            for (int y = 0; y < blockSize.y; y++)
            for (int x = 0; x < blockSize.x; x++)
            {
                var pos = tilePosition + new TilePosition(x, y);
                if (!GetBlockAt(pos).IsAir()) return false;
            }

            return true;
        }

        public bool CanPlaceCropAt(TilePosition tilePosition, string blockId)
        {
            var belowBlock = GetBlockBelow(tilePosition);
            return CanPlaceAt(tilePosition, blockId) && belowBlock.Id() == BlockIds.Dirt;
        }

        public bool CanBreakAt(TilePosition tilePosition) =>
            GetBlockAt(tilePosition).GetBlockData().IsDestructible;

        public bool IsInsideCollider(TilePosition givenPos, ICollider collider) =>
            GetPositionsColliderIn(collider).Any(pos => pos == givenPos);

        public bool TryGetInteractable(WorldPosition pos, out IInteractable interactable)
        {
            var tilePos = pos.ToTilePosition();
            var blockEntity = GetTileEntityAt(tilePos);
            if (blockEntity == null)
            {
                GameLogger.Log("Block is null","TryGetInteractable");
                interactable = null;
                return false;
            }

            interactable = blockEntity;
            return true;
        }
        #endregion
        
        #region Block Queries
        public List<TilePosition> GetPositionsColliderIn(ICollider hitbox)
        {
            var blocks = new List<TilePosition>();
            var bounds = hitbox.Bounds.ToBounds2DIntInclusive();

            for (int x = bounds.MinX; x <= bounds.MaxX; x++)
            for (int y = bounds.MinY; y <= bounds.MaxY; y++)
            {
                var pos = new TilePosition(x, y);
                if (!IsInsideBounds(pos)) 
                    continue;
                blocks.Add(pos);
            }

            return blocks;
        }
        #endregion

        #region Tickable Management
      
        public void Tick(float timeInterval, TickContext ctx)
        {
            foreach (var (_, tileEntity) in _blockEntities)
            {
               tileEntity.Tick(timeInterval, ctx);
            }
        }
        #endregion

        private void AddBlockEntity(BlockEntity blockEntity)
        {
            _blockEntities[blockEntity.Position] = blockEntity;
        }

        public BlockEntity GetBlockEntity(TilePosition tilePosition) 
            => _blockEntities.GetValueOrDefault(tilePosition);
    }
    
}