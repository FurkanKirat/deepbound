using System;
using System.Collections.Generic;
using Core.Context;
using Core.Context.Registry;
using Data.Database;
using Data.Models.Blocks.Behaviors;
using Data.RegistrySystem;
using GameLoop;
using Systems.EntitySystem.Interfaces;
using Systems.Physics.Colliders;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Systems.WorldSystem;
using Utils.Extensions;

namespace Data.Models.Blocks
{
    public class BlockEntity : 
        ISaveable<BlockEntitySaveData>,
        ITickable,
        IInteractable,
        ICollidable,
        IDisposable
    {
        public BlockData BlockData { get; }
        public TilePosition Position { get; }
        public int Width { get; }
        public int Height { get; }
        public List<IBlockBehavior> Behaviors { get; }
        public ICollider Collider { get; }
        
        private BlockEntity(BlockData blockData, TilePosition position, int width, int height, List<IBlockBehavior> behaviors)
        {
            BlockData = blockData;
            Position = position;
            Width = width;
            Height = height;
            Behaviors = behaviors;
            Collider = AABBCollider.GetBlockAABB(position, width, height);
        }

        public static BlockEntity Create(TilePosition pos, string blockId, bool isGenerated = false)
        {
            var blockData = Databases.Blocks[blockId];
            var width = blockData.Size.x;
            var height = blockData.Size.y;
            
            var behaviors = new List<IBlockBehavior>();
            
            var blockBehaviorCtx = new BlockBehaviorContext
            {
                Position = pos,
                BlockData = blockData,
                IsGenerated = isGenerated,
            };
            
            if (blockData.HasAnyBehavior)
            {
                foreach (var behavior in blockData.Behaviors)
                {
                    behaviors.Add(behavior?.Create(blockBehaviorCtx));
                }
            }
            
            
            return new BlockEntity(blockData, pos, width, height, behaviors);
        }
        public static BlockEntity Load(BlockEntitySaveData saveData)
        {
            var blockData = Databases.Blocks[saveData.BlockId];
            var behaviors = new List<IBlockBehavior>();
            var behaviorCtx = new BlockBehaviorContext { Position = saveData.Position, BlockData = blockData };
            foreach (var savedBehavior in saveData.Behaviors)
            {
                var behavior = Registries.BlockBehaviorFactory.Load(
                    savedBehavior.BehaviorId, behaviorCtx, savedBehavior);
                behaviors.Add(behavior);
            }
            return new BlockEntity(blockData, saveData.Position, saveData.Width, saveData.Height, behaviors);
        }

        public void Dispose()
        {
            foreach(var behavior in Behaviors)
                behavior.Dispose();
        }

        public BlockEntitySaveData ToSaveData()
        {
            var behaviors = new List<BlockBehaviorSaveData>();
            foreach (var behavior in Behaviors)
                behaviors.Add(behavior.ToSaveData());

            return new BlockEntitySaveData
            {
                BlockId = BlockData.Id,
                Width = Width,
                Height = Height,
                Position = Position,
                Behaviors = behaviors
            };
        }
        
        public void Tick(float dt, TickContext ctx)
        {
            foreach (var b in Behaviors) b.Tick(dt, ctx);
        }

        public void Interact(IPlayer player, World world)
        {
            foreach (var b in Behaviors) b.Interact(player, world);
        }
        
        public void OnCollisionWithEntity(IPhysicalEntity entity)
        {
            foreach (var b in Behaviors) b.OnCollisionWithEntity(entity);
        }

        public void OnRemove(World world)
        {
            foreach (var b in Behaviors) b.OnRemove(world);
        }

        public bool HasBehavior<T>()
        {
            foreach (var behavior in Behaviors)
            {
                if (behavior is T)
                    return true;
            }
            return false;
        }

        public T GetBehavior<T>()
        {
            foreach (var behavior in Behaviors)
            {
                if (behavior is T givenBehavior)
                    return givenBehavior;
            }
            return default;
        }

        public bool TryGetBehavior<T>(out T givenBehavior)
        {
            foreach (var behavior in Behaviors)
            {
                if (behavior is T tBehavior)
                {
                    givenBehavior = tBehavior;
                    return true;
                }
            }
            
            givenBehavior = default;
            return false;
        }

        public override string ToString()
        {
            return
                $"{nameof(Position)}: {Position}, {nameof(Width)}: {Width}, {nameof(Height)}: {Height}, {nameof(Behaviors)}: {Behaviors.ToDebugString()}";
        }

        
    }

}