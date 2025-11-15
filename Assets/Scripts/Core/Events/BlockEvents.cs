using System.Collections.Generic;
using Data.Models;
using Data.Models.Blocks;
using Data.Models.Items;
using Systems.EntitySystem.Interfaces;

namespace Core.Events
{
    public readonly struct BlockBreakRequest : IEvent
    {
        public readonly Block Block;
        public readonly TilePosition BlockPosition;
        public readonly ItemInstance ItemInstance;
        public readonly IPlayer Player;

        public BlockBreakRequest(Block block, TilePosition blockPos, ItemInstance itemInstance, IPlayer player)
        {
            Block = block;
            BlockPosition = blockPos;
            ItemInstance = itemInstance;
            Player = player;
        }
    }
    
    public readonly struct BlockBreakingProgress : IEvent
    {
        public readonly TilePosition BlockPosition;
        public readonly Block Block;
        public readonly float Progress; // 0.0f to 1.0f

        public BlockBreakingProgress(TilePosition blockPos, Block block, float progress)
        {
            BlockPosition = blockPos;
            Block = block;
            Progress = progress;
        }
        
        
    }
    public readonly struct BlockDestroyedEvent : IEvent
    {
        public List<TilePosition> Positions { get; }

        public BlockDestroyedEvent(List<TilePosition> positions)
        {
            Positions = positions;
        }
        
        public BlockDestroyedEvent(TilePosition position) : this(new List<TilePosition> { position }) { }
    }

    public readonly struct BlockBreakCancelledEvent : IEvent
    {
        public readonly TilePosition TilePosition;

        public BlockBreakCancelledEvent(TilePosition pos)
        {
            TilePosition = pos;
        }
        
    }
    
    public readonly struct BlockPlacedEvent : IEvent
    {
        public List<TilePosition> Positions { get; }

        public BlockPlacedEvent(List<TilePosition> positions)
        {
            Positions = positions;
        }
    }
    

}

