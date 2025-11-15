using System.Collections.Generic;
using Constants;
using Core;
using Core.Events;
using Data.Database;
using Data.Models;
using Systems.WorldSystem;
using UnityEngine;
using Utils;

namespace Visuals.Rendering
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BlockRenderer : MonoBehaviour
    {
        private readonly List<Vector2> _uvs = new();
        private const float TileSize = TileConstants.TileLength;
        private World _world;
        private Mesh _mesh;
        private BlockAnimator _animator;

        private Dimension Dimension => _world.CurrentDimension;
        public void OnEnable()
        {
            GameEventBus.Subscribe<BlockDestroyedEvent>(OnBlockDestroyed);
            GameEventBus.Subscribe<BlockPlacedEvent>(OnBlockPlaced);
            GameEventBus.Subscribe<CropStateChangedEvent>(OnGrowthChanged);
            GameEventBus.Subscribe<DimensionChangedEvent>(OnDimensionChanged);
        }

        public void OnDisable()
        {
            GameEventBus.Unsubscribe<BlockDestroyedEvent>(OnBlockDestroyed);
            GameEventBus.Unsubscribe<BlockPlacedEvent>(OnBlockPlaced);
            GameEventBus.Unsubscribe<CropStateChangedEvent>(OnGrowthChanged);
            GameEventBus.Unsubscribe<DimensionChangedEvent>(OnDimensionChanged);
        }

        public void Init(World world)
        {
            _world = world;
            GenerateMesh();
            SetMaterial();
            _animator = new BlockAnimator(this, _world);
        }

        private void GenerateMesh()
        {
            if (_mesh != null)
                Destroy(_mesh);
            _mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };
            List<Vector3> vertices = new();
            List<int> triangles = new();

            int vertexIndex = 0;

            _uvs.Clear();
            for (int y = 0; y < Dimension.BlockManager.Height; y++)
            for (int x = 0; x < Dimension.BlockManager.Width; x++)
            {
                var tilePos = new TilePosition(x, y);
                GenerateTileMeshData(tilePos, vertices, triangles, ref vertexIndex);
            }

            _mesh.SetVertices(vertices);
            _mesh.SetUVs(0, _uvs);
            _mesh.SetTriangles(triangles, 0);
            _mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = _mesh;
        }

        public void SetTileSprite(TilePosition tilePosition, int frame)
        {
            var block = Dimension.BlockManager.GetBlockAt(tilePosition);
            string textureId = _world.BlockManager.GetTextureId(tilePosition);
            Rect uvRect = AtlasUVIndex.GetUV(textureId, frame, block.OffsetX, block.OffsetY);
            UpdateTileUV(uvRect, tilePosition);
        }
        
        private void OnBlockPlaced(BlockPlacedEvent evt)
        {
            foreach (var pos in evt.Positions)
            {
                UpdateTileUV(pos);
            }
        }

        private void OnBlockDestroyed(BlockDestroyedEvent evt)
        {
            foreach (var pos in evt.Positions)
            {
                UpdateTileUV(pos);
            }
        }

        private void OnGrowthChanged(CropStateChangedEvent evt)
        {
            UpdateTileUV(evt.Position);
        }

        private void OnDimensionChanged(DimensionChangedEvent evt)
        {
            GenerateMesh();
        }
        
        private void GenerateTileMeshData(TilePosition tilePosition, List<Vector3> vertices, List<int> triangles,
            ref int vertexIndex)
        {
            var tile = Dimension.BlockManager.GetBlockAt(tilePosition);

            var textureId = _world.BlockManager.GetTextureId(tilePosition);
            Rect uvRect = AtlasUVIndex.GetUV(textureId, 0, tile.OffsetX, tile.OffsetY);

            Vector3 bottomLeft = new Vector3(tilePosition.X * TileSize, tilePosition.Y * TileSize, 0);
            Vector3 bottomRight = bottomLeft + new Vector3(TileSize, 0, 0);
            Vector3 topRight = bottomLeft + new Vector3(TileSize, TileSize, 0);
            Vector3 topLeft = bottomLeft + new Vector3(0, TileSize, 0);

            vertices.Add(bottomLeft);
            vertices.Add(bottomRight);
            vertices.Add(topRight);
            vertices.Add(topLeft);

            _uvs.Add(new Vector2(uvRect.xMin, uvRect.yMin));
            _uvs.Add(new Vector2(uvRect.xMax, uvRect.yMin));
            _uvs.Add(new Vector2(uvRect.xMax, uvRect.yMax));
            _uvs.Add(new Vector2(uvRect.xMin, uvRect.yMax));

            triangles.Add(vertexIndex + 0);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex + 0);

            vertexIndex += 4;
        }

        private void UpdateTileUV(TilePosition tilePosition, int frame = 0)
        {
            if (_mesh == null) return;
            
            var block = Dimension.BlockManager.GetBlockAt(tilePosition);
            var textureId = _world.BlockManager.GetTextureId(tilePosition);
            Rect uvRect = AtlasUVIndex.GetUV(textureId, frame, block.OffsetX, block.OffsetY);
            
            UpdateTileUV(uvRect, tilePosition);
        }
        
        
        private void UpdateTileUV(Rect uvRect, TilePosition tilePosition)
        {
            if (_mesh == null) return;

            int tileIndex = tilePosition.Y * Dimension.BlockManager.Width + tilePosition.X;
            int vertexStart = tileIndex * 4;
            
            _uvs[vertexStart + 0] = new Vector2(uvRect.xMin, uvRect.yMin);
            _uvs[vertexStart + 1] = new Vector2(uvRect.xMax, uvRect.yMin);
            _uvs[vertexStart + 2] = new Vector2(uvRect.xMax, uvRect.yMax);
            _uvs[vertexStart + 3] = new Vector2(uvRect.xMin, uvRect.yMax);

            _mesh.SetUVs(0, _uvs);
        }

        
        private void SetMaterial()
        {
            var material = new Material(Shader.Find("Sprites/Default"));
            Texture2D atlasTexture = Resources.Load<Texture2D>("Atlas/tileAtlas");

            if (atlasTexture == null)
            {
                GameLogger.Error("❌ tileAtlas not found in Resources/Sprites/", nameof(BlockRenderer));
                return;
            }

            material.mainTexture = atlasTexture;
            GetComponent<MeshRenderer>().material = material;
        }

    }

}

