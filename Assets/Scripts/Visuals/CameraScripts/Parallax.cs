using System.Collections.Generic;
using Core;
using Core.Context;
using Core.Events;
using Interfaces;
using Visuals.Interfaces;
using Visuals.Utils;

namespace Visuals.CameraScripts
{
    using UnityEngine;

    public class Parallax : MonoBehaviour,
        IClientLateTickable,
        IInitializable<ClientContext>
    {
        [SerializeField] private ParallaxLayerVisual parallaxLayerVisualPrefab;
        [SerializeField] private float baseSpeed = 5f;
        private ObjectPool<ParallaxLayerVisual> _parallaxLayerPool;
        private bool _isInitialized;
        private CameraManager _cameraManager;
        private readonly List<ParallaxLayerVisual> _parallaxLayers = new();
        
        private void OnEnable()
        {
            ClientGameLoop.Instance.Register(this);
            GameEventBus.Subscribe<WorldLayerChangedEvent>(OnWorldLayerChanged);
        }

        private void OnDisable()
        {
            ClientGameLoop.Instance?.Unregister(this);
            GameEventBus.Unsubscribe<WorldLayerChangedEvent>(OnWorldLayerChanged);
        }

        private void OnWorldLayerChanged(WorldLayerChangedEvent e)
        {
            _parallaxLayerPool.ReleaseAll();
            _parallaxLayers.Clear();

            var parallaxLayers = e.WorldLayer.ParallaxLayers;
            int sortingOrder = -parallaxLayers.Count;
            foreach (var parallaxLayer in parallaxLayers)
            {
                var parallaxLayerVisual = _parallaxLayerPool.Get();

                parallaxLayerVisual.Initialize(
                    parallaxLayer.Name,
                    _cameraManager,
                    parallaxLayer.Sprite.Load(), 
                    parallaxLayer.Factor,
                    baseSpeed,
                    sortingOrder);
                sortingOrder++;
                _parallaxLayers.Add(parallaxLayerVisual);
            }
        }

        public void Initialize(ClientContext data)
        {
            _parallaxLayerPool ??= new ObjectPool<ParallaxLayerVisual>(parallaxLayerVisualPrefab, 4, transform);
            _cameraManager = data.CameraManager;
            _isInitialized = true;
        }
        
        public void ClientLateTick(float deltaTime)
        {
            if (!_isInitialized || _cameraManager.State != CameraState.Following)
                return;
            
            foreach (var parallaxLayer in _parallaxLayers)
            {
                parallaxLayer.UpdateMovement(deltaTime);
            }
        }
    }

}