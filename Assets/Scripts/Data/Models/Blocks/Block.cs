using System;
using System.Collections.Generic;
using Data.Database;

namespace Data.Models.Blocks
{
    public readonly struct Block : IEquatable<Block>
    {
        public readonly ushort BlockType;
        public readonly byte OffsetX;
        public readonly byte OffsetY;

        private Block(ushort blockType, byte offsetX, byte offsetY)
        {
            BlockType = blockType;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }
        
        public bool IsMaster => OffsetX == 0 && OffsetY == 0;
        
        public static Block CreateMaster(string blockId)
        {
            return new Block(BlockIdCache.GetUshort(blockId), 0, 0);
        }

        public static Block CreateMaster(ushort blockType)
        {
            return new Block(blockType, 0, 0);
        }

        public static Block CreateSlave(ushort blockType, byte offsetX, byte offsetY)
        {
            return new Block(blockType, offsetX, offsetY);
        }
        
        public static Block CreateSlave(string blockId, byte offsetX, byte offsetY)
        {
            return new Block(BlockIdCache.GetUshort(blockId), offsetX, offsetY);
        }

        public static List<Block> CreateSlaves(ushort blockType)
        {
            var slaves = new List<Block>();
            var blockData = BlockIdCache.GetBlockData(blockType);
            for (byte y = 0; y < blockData.Size.y; y++)
            for (byte x = 0; x < blockData.Size.x; x++)
            {
                if (x == 0 && y == 0)
                    continue;
                var slave = CreateSlave(blockType, x, y);
                slaves.Add(slave);
            }
             
            return slaves;
        }
        
        public static List<Block> CreateMasterAndSlaves(ushort blockType)
        {
            var blocks = new List<Block>();
            var blockData = BlockIdCache.GetBlockData(blockType);
            for (byte y = 0; y < blockData.Size.y; y++)
            for (byte x = 0; x < blockData.Size.x; x++)
            {
                var block = x == 0 && y == 0 ? 
                    CreateMaster(blockType) : 
                    CreateSlave(blockType, x, y);
                    
                blocks.Add(block);
            }
             
            return blocks;
        }

        public bool Equals(Block other)
        {
            return BlockType == other.BlockType && OffsetX == other.OffsetX && OffsetY == other.OffsetY;
        }

        public override bool Equals(object obj)
        {
            return obj is Block other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BlockType, OffsetX, OffsetY);
        }
        
        public bool SameType(Block other)
        {
            return BlockType == other.BlockType;
        }
        
        public static bool operator ==(Block left, Block right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Block left, Block right)
        {
            return !left.Equals(right);
        }
    }
}