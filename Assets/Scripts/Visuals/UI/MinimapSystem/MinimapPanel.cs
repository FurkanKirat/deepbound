using System.Collections.Generic;
using Core;
using Core.Events;
using Systems.EntitySystem.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Visuals.Interfaces;
using Visuals.Utils;

namespace Visuals.UI.MinimapSystem
{
    public class MinimapPanel : MonoBehaviour, IClientLateTickable
    {
        [Header("References")]
        [SerializeField] private RawImage minimapImage;
        [SerializeField] private RectTransform minimapContainer;
        [SerializeField] private RectTransform minimapPanel;
        
        [Header("Settings")]
        [SerializeField] private Image iconPrefab;
        [SerializeField] private float maxZoom;
        
        private ObjectPool<Image> _pool;
        private readonly Dictionary<IPhysicalEntity, Image> _entities = new();
        private float _widthRatio;
        private float _heightRatio;
        private float _worldWidth;
        private float _worldHeight;
        
        private float _zoom = 1f;
        private bool _isDragging = false;
        private Vector2 _lastMousePos;

        private void OnEnable()
        {
            ClientGameLoop.Instance.Register(this);
            GameEventBus.Subscribe<UIPrimaryUseStarted>(OnUIPrimaryUseStarted);
            GameEventBus.Subscribe<UIPrimaryUseHeld>(OnUIPrimaryUseHeld);
            GameEventBus.Subscribe<UIPrimaryUseEnded>(OnUIPrimaryUseEnded);
            GameEventBus.Subscribe<MouseScrolledEvent>(OnMouseScrolled);
        }
        
        private void OnDisable()
        {
            ClientGameLoop.Instance?.Unregister(this);
            GameEventBus.Unsubscribe<UIPrimaryUseStarted>(OnUIPrimaryUseStarted);
            GameEventBus.Unsubscribe<UIPrimaryUseHeld>(OnUIPrimaryUseHeld);
            GameEventBus.Unsubscribe<UIPrimaryUseEnded>(OnUIPrimaryUseEnded);
            GameEventBus.Unsubscribe<MouseScrolledEvent>(OnMouseScrolled);
        }

        public void UpdateMinimap(Texture2D texture, IEnumerable<IPhysicalEntity> entities)
        {
            _pool ??= new ObjectPool<Image>(iconPrefab, 10, minimapImage.transform);
            HashSet<IPhysicalEntity> activeEntities = new();
            
            var screenHeight = Screen.height;
            
            _worldWidth  = texture.width;
            _worldHeight = texture.height;
            _widthRatio = minimapPanel.rect.width / _worldWidth;
            _heightRatio = minimapPanel.rect.height / _worldHeight;
            
            minimapPanel.sizeDelta = new Vector2(screenHeight * _worldWidth / _worldHeight, minimapPanel.sizeDelta.y);
            minimapImage.texture = texture;
            
            foreach (var entity in entities)
            {
                if (!entity.EntityData.ShowInMap)
                    continue;
                
                activeEntities.Add(entity);
                UpdateEntity(entity, true);
            }

            // Release unused
            var toRemove = new List<IPhysicalEntity>();
            foreach (var kvp in _entities)
            {
                if (!activeEntities.Contains(kvp.Key))
                {
                    _pool.Release(kvp.Value);
                    toRemove.Add(kvp.Key);
                }
            }
            foreach (var entity in toRemove)
                _entities.Remove(entity);
        }

        private void UpdateEntity(IPhysicalEntity entity, bool updateSprite)
        {
            if (!_entities.TryGetValue(entity, out var image))
            {
                image = _pool.Get();
                _entities[entity] = image;
            }

            var posX = entity.Position.x - _worldWidth / 2f;
            var posY = entity.Position.y - _worldHeight / 2f;
                
            var pos = new Vector3(posX * _widthRatio, posY * _heightRatio);
                
            image.transform.localPosition = pos;
            if (updateSprite)
                image.sprite = entity.EntityData.MapIcon.Load();
            
        }

        public void ClientLateTick(float deltaTime)
        {
            foreach (var entity in _entities.Keys)
            {
                UpdateEntity(entity, false);
            }
        }
        
        private void OnMouseScrolled(MouseScrolledEvent e)
        {
            SetZoom(e.ScrollValue + _zoom);
        }
        
        private void SetZoom(float newZoom)
        {
            float oldZoom = _zoom;
            _zoom = Mathf.Max(Mathf.Min(maxZoom, newZoom), 1f);

            var zoomScale = Vector3.one * _zoom;
            minimapImage.rectTransform.localScale = zoomScale;
            
            foreach (var kvp in _entities)
            {
                kvp.Value.rectTransform.localScale = zoomScale;
            }
            
            minimapImage.rectTransform.anchoredPosition *= _zoom / oldZoom;

            ClampMinimapPosition();
        }
        
        private void OnUIPrimaryUseStarted(UIPrimaryUseStarted e)
        {
            _isDragging = true;
            _lastMousePos = e.ScreenPosition;
        }

        private void OnUIPrimaryUseHeld(UIPrimaryUseHeld e)
        {
            if (!_isDragging) return;

            var delta = e.ScreenPosition - _lastMousePos;
            _lastMousePos = e.ScreenPosition;

            minimapImage.rectTransform.anchoredPosition += delta;

            ClampMinimapPosition();
        }

        private void OnUIPrimaryUseEnded(UIPrimaryUseEnded e)
        {
            _isDragging = false;
        }
        
        private void ClampMinimapPosition()
        {
            var containerRect = minimapContainer.rect;
            var imageRect = minimapImage.rectTransform.rect;

            var scaledWidth  = imageRect.width * minimapImage.rectTransform.localScale.x;
            var scaledHeight = imageRect.height * minimapImage.rectTransform.localScale.y;

            var pos = minimapImage.rectTransform.anchoredPosition;

            float limitX = Mathf.Max(0, (scaledWidth - containerRect.width) / 2f);
            float limitY = Mathf.Max(0, (scaledHeight - containerRect.height) / 2f);

            pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
            pos.y = Mathf.Clamp(pos.y, -limitY, limitY);

            minimapImage.rectTransform.anchoredPosition = pos;
        }
    }
}