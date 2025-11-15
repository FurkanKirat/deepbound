using Core;
using Core.Context;
using Core.Events;
using Data.Models.Blocks;
using Interfaces;
using Systems.WorldSystem;
using UnityEngine;

namespace Visuals.UI.MinimapSystem
{
    public class MinimapUIController : 
        BasePanelUIController,
        IInitializable<ClientContext>
    {
        [SerializeField] private MinimapPanel minimapPanel;
        public override bool BlocksWorldInteraction => true;
        public override bool PausesGame => true;
        public override UIPanelType PanelType => UIPanelType.Minimap;
        private World _world;
        private Texture2D _minimapTexture;
        protected override string OpenSound => null;
        protected override string CloseSound => null;

        private void OnEnable()
        {
            GameEventBus.Subscribe<MinimapToggleRequested>(OnToggleRequested);
            GameEventBus.Subscribe<BlockDestroyedEvent>(OnBlockDestroyed);
            GameEventBus.Subscribe<BlockPlacedEvent>(OnBlockPlaced);
            GameEventBus.Subscribe<DimensionChangedEvent>(OnDimensionChanged);
            GameEventBus.Subscribe<MinimapChunkDiscoveredEvent>(OnChunkDiscovered);
        }
        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<MinimapToggleRequested>(OnToggleRequested);
            GameEventBus.Unsubscribe<BlockDestroyedEvent>(OnBlockDestroyed);
            GameEventBus.Unsubscribe<BlockPlacedEvent>(OnBlockPlaced);
            GameEventBus.Unsubscribe<DimensionChangedEvent>(OnDimensionChanged);
            GameEventBus.Unsubscribe<MinimapChunkDiscoveredEvent>(OnChunkDiscovered);
        }

        private void OnDimensionChanged(DimensionChangedEvent obj)
        {
            UpdateMap();
            UpdateTextureIfOpen();
        }

        private void OnBlockDestroyed(BlockDestroyedEvent e)
        {
            foreach (var pos in e.Positions)
            {
                UpdateBlock(pos.X, pos.Y);
            }
            
            UpdateTextureIfOpen();
        }
        
        private void OnBlockPlaced(BlockPlacedEvent e)
        {
            foreach (var pos in e.Positions)
            {
                UpdateBlock(pos.X, pos.Y);
            }
            
            UpdateTextureIfOpen();
        }

        private void OnToggleRequested(MinimapToggleRequested e)
        {
            _minimapTexture.Apply();
            minimapPanel.UpdateMinimap(_minimapTexture, _world.EntityManager.AllEntities);
            Toggle();
        }
        
        private void OnChunkDiscovered(MinimapChunkDiscoveredEvent e)
        {
            UpdateChunk(e.ChunkX, e.ChunkY);
            UpdateTextureIfOpen();
        }

        public void Initialize(ClientContext data)
        {
            _world = data.World;
            UpdateMap();
        }
        
        private void UpdateMap()
        {
            var dim = _world.CurrentDimension;
            var blockManager = dim.BlockManager;
            _minimapTexture = new Texture2D(blockManager.Width, blockManager.Height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            for(int x = 0; x < blockManager.Width; x++)
            for (int y = 0; y < blockManager.Height; y++)
            {
                UpdateBlock(x, y);
            }
            
        }

        private void UpdateBlock(int x, int y)
        {
            var dim = _world.CurrentDimension;
            var color = Color.black;
            var blockManager = dim.BlockManager;
            if (dim.MinimapDiscovery.IsBlockDiscovered(x,y))
                color = blockManager.GetBlockAt(x,y).GetBlockData().MapColor.Load();
                
            _minimapTexture.SetPixel(x, y, color);
        }

        private void UpdateChunk(int chunkX, int chunkY)
        {
            var dim = _world.CurrentDimension;
            var color = Color.black;
            var blockManager = dim.BlockManager;
            var minimap = dim.MinimapDiscovery;

            const int chunkSize = MinimapDiscovery.ChunkSize;
            for (int offsetX = 0; offsetX < chunkSize; offsetX++)
            for (int offsetY = 0; offsetY < chunkSize; offsetY++)
            {
                int x = chunkX * chunkSize + offsetX;
                int y = chunkY * chunkSize + offsetY;
                if (!blockManager.IsInsideBounds(x, y))
                    continue;
                
                if (minimap.IsChunkDiscovered(chunkX, chunkY))
                    color = blockManager.GetBlockAt(x,y).GetBlockData().MapColor.Load();
                
                _minimapTexture.SetPixel(x, y, color);
            }
            
        }

        private void UpdateTextureIfOpen()
        {
            if (IsOpen)
                _minimapTexture.Apply();
        }
    }
}