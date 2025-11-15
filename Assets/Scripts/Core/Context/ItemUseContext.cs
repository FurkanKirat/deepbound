using Data.Models;
using Data.Models.Items;
using Systems.EntitySystem;
using Systems.EntitySystem.Interfaces;
using Systems.WorldSystem;
using UnityEngine;
using Random = System.Random;

namespace Core.Context
{
    public class ItemUseContext
    {
        public IPlayer User { get; set; }
        public ItemInstance Item { get; set; }
        public WorldPosition TargetPosition { get; set; }
        public TilePosition TargetTilePosition { get; set; }
        public Vector2 ScreenPosition { get; set; }
        
        public World World { get; set; }
        public Dimension Dimension => World.CurrentDimension;
        public BlockManager BlockManager => Dimension.BlockManager;
        public EntityManager EntityManager => Dimension.EntityManager;
        public Random Random => World.Random;
    }

}