using Core;
using Core.Context;
using Core.Events;
using Data.Models;
using Data.Models.Items;
using Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.InputSystem;
using UnityEngine;
using Visuals.CameraScripts;
using Visuals.Interfaces;

namespace Visuals.Rendering
{
    public class BlockCrosshair : MonoBehaviour, IClientTickable, IInitializable<ClientContext>
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer overlay;
        [SerializeField] private CameraManager cameraManager;
        
        [Header("Colors")]
        [SerializeField] private Color validColor;
        [SerializeField] private Color invalidColor;
        
        private TilePosition _currentPosition;
        private BlockRaycaster _raycaster;
        private IPlayer _player;

        private bool Enabled
        {
            get => overlay.enabled;
            set
            {
                if (Enabled != value)
                    overlay.enabled = value;
            }
        }

        private TilePosition Position
        {
            get => _currentPosition;
            set
            {
                if (_currentPosition != value)
                {
                    _currentPosition = value;
                    transform.position = value.ToVector3();
                }
            }
        }

        private Color Color
        {
            get => overlay.color;
            set
            {
                if (Color != value)
                    overlay.color = value;
            }
        }
        
        private void OnEnable()
        {
            GameEventBus.Subscribe<SelectedSlotChangedEvent>(OnSelectedSlotChanged);
            ClientGameLoop.Instance.Register(this);
        }

        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<SelectedSlotChangedEvent>(OnSelectedSlotChanged);
            ClientGameLoop.Instance?.Unregister(this);
        }

        private void OnSelectedSlotChanged(SelectedSlotChangedEvent e)
        {
            Enabled = e.SelectedItem.IsCrosshairOpen();
        }

        public void ClientTick(float deltaTime)
        {
            if (!Enabled)
                return;

            _raycaster ??= new BlockRaycaster(cameraManager.MainCamera);
            Position = _raycaster.GetWorldPosition(Input.mousePosition).ToTilePosition();
            
            Color = (_player.Position - Position.ToWorldPosition()).SqrMagnitude > _player.Config.BlockBreakingRangeSqr ? invalidColor : validColor;
        }

        public void Initialize(ClientContext data)
        {
            _player = data.Player;
        }
    }
}