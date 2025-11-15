using Core;
using Core.Context.Spawn;
using Core.Events;
using Data.Models;
using Data.Models.Blocks;
using UnityEngine;

namespace Visuals.Rendering
{
    public class BlockBreakingVisual : MonoBehaviour
    {
        [SerializeField] private Sprite[] blockSprites;
        [SerializeField] private SpriteRenderer blockRenderer;
        private bool _isEnabled = false;
        private int _currentIndex = -1;

        private void OnEnable()
        {
            GameEventBus.Subscribe<BlockBreakingProgress>(OnProgress);
            GameEventBus.Subscribe<BlockBreakCancelledEvent>(OnCancelled);
        }

        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<BlockBreakingProgress>(OnProgress);
            GameEventBus.Unsubscribe<BlockBreakCancelledEvent>(OnCancelled);
        }
        
        private void OnCancelled(BlockBreakCancelledEvent e)
        {
            _isEnabled = false;
            blockRenderer.enabled = false;
        }


        private void OnProgress(BlockBreakingProgress e)
        {
            SetStage(e.Progress, e.Block, e.BlockPosition);
        }

        private void SetStage(float progress, Block block, TilePosition tilePos)
        {
            if (progress >= 1f)
            {
                blockRenderer.enabled = false;
                _isEnabled = false;
                return;
            }

            if (!_isEnabled)
            {
                blockRenderer.enabled = true;
                _isEnabled = true;
            }

            var position = tilePos.ToVector3();
            var blockData = block.GetBlockData();
            
            int stageCount = blockSprites.Length;

            int index = Mathf.Clamp((int)(progress * stageCount), 0, stageCount - 1);

            if (index != _currentIndex)
            {
                blockRenderer.sprite = blockSprites[index];
                _currentIndex = index;

                var color = blockData.MapColor.Load();
                
                var baseColor = blockData.MapColor.Load();
                Color.RGBToHSV(baseColor, out var hue, out var sat, out var val);
                
                hue += Random.Range(-0.02f, 0.02f);
                sat *= Random.Range(0.9f, 1.1f);
                val *= Random.Range(0.9f, 1.1f);

                var variedColor = Color.HSVToRGB(hue, sat, val);
                
                GameEventBus.Publish(new TrailSpawnRequest
                {
                    SpawnContext = new TrailSpawnContext
                    {
                        Position = position,
                        Rotation = Quaternion.identity,
                        StartScale = 0.2f,
                        FinalScale = 0.1f,
                        StartColor = new Color(variedColor.r, variedColor.g, variedColor.b, 1f),
                        FinalColor = new Color(variedColor.r, variedColor.g, variedColor.b, 0f),
                        LifeTime = 0.3f,
                        
                        Count = 4,
                        SpreadAngle = 360f,
                        SpeedMin = 1f,
                        SpeedMax = 2f
                    }
                });
            }

            blockRenderer.transform.position = position;

        }

        
    }
}