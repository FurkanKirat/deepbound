using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Core;
using Core.Context;
using Core.Events;
using Data.Database;
using Data.Models.Crafting;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using Visuals;
using Visuals.Interfaces;
using Visuals.UI;

namespace Systems.InputSystem
{
    public class InputManager : IClientTickable, IDisposable
    {
        private readonly IPlayer _player;
        private readonly BlockRaycaster _blockRaycaster;
        
        private bool _isPrimaryUseHeld = false;
        private float _primaryUseHoldTime = 0f;
        private float _primaryUseHeldIntervalTimer = 0f;
        
        private static float HeldEventInterval => Configs.GameConfig.Input.HeldEventInterval;
        private static float SlotSelectCooldown => Configs.GameConfig.Input.SlotSelectCooldown;
        public InputManager(ClientContext client)
        {
            _player = client.Player;
            
            _blockRaycaster = new BlockRaycaster(client.CameraManager.MainCamera);
            
            ClientGameLoop.Instance.Register(this);
        }

        public void Dispose()
        {
            ClientGameLoop.Instance?.Unregister(this);
        }

        public void ClientTick(float timeInterval)
        {
            HandleUIInput();

            HandleActionInput(timeInterval);

            bool isAnyBlockingPanelOpen = UIPanelManager.Instance.IsAnyBlockingPanelOpen;

            InputCache.Update(isAnyBlockingPanelOpen);
            HandleInventorySlotInput(isAnyBlockingPanelOpen);
        }

        private void HandleUIInput()
        {
            if (InputBindingManager.Bindings.GetBinding(InputAction.Escape).IsHeld)
                GameEventBus.Publish(new EscapePressedEvent());

            if (InputBindingManager.Bindings.GetBinding(InputAction.OpenChat).IsHeld)
                GameEventBus.Publish(new ChatSubmitOrToggleEvent());
            
            if (InputBindingManager.Bindings.GetBinding(InputAction.Screenshot).IsHeld)
                ScreenshotManager.TakeScreenshot();
            
            if (InputBindingManager.Bindings.GetBinding(InputAction.OpenMinimap).IsHeld)
                GameEventBus.Publish(new MinimapToggleRequested());
            
            if (InputBindingManager.Bindings.GetBinding(InputAction.OpenInventory).IsHeld)
                GameEventBus.Publish(new InventoryOpenRequestEvent(
                    new InventoryOpenConfig(_player, new [] { SlotCollectionType.Equipment, SlotCollectionType.Accessory, SlotCollectionType.Inventory })));

            if (InputBindingManager.Bindings.GetBinding(InputAction.OpenCrafting).IsHeld)
                GameEventBus.Publish(new InventoryOpenRequestEvent(
                    new InventoryOpenConfig(_player, new [] { SlotCollectionType.Inventory }), 
                    CraftingStation.Hand));

        }

        private void HandleActionInput(float timeInterval)
        {
            var primaryUseKey = InputBindingManager.Bindings.GetBinding(InputAction.PrimaryUse);

            if (primaryUseKey.IsHeld)
            {
                var screenPos = Input.mousePosition;
                var worldPos = _blockRaycaster.GetWorldPosition(screenPos);

                _isPrimaryUseHeld = true;
                _primaryUseHoldTime = 0f;
                _primaryUseHeldIntervalTimer = 0f;

                if (EventSystem.current.IsPointerOverGameObject())
                    GameEventBus.Publish(new UIPrimaryUseStarted(screenPos));
                else
                    GameEventBus.Publish(new PrimaryUseStarted(screenPos, worldPos));
            }

            if (_isPrimaryUseHeld)
            {
                _primaryUseHoldTime += timeInterval;
                _primaryUseHeldIntervalTimer += timeInterval;

                if (_primaryUseHeldIntervalTimer >= HeldEventInterval)
                {
                    _primaryUseHeldIntervalTimer = 0f;
                    var screenPos = Input.mousePosition;
                    var worldPos = _blockRaycaster.GetWorldPosition(screenPos);

                    if (EventSystem.current.IsPointerOverGameObject())
                        GameEventBus.Publish(new UIPrimaryUseHeld(screenPos));
                    else
                        GameEventBus.Publish(new PrimaryUseHeld(screenPos, worldPos));
                }
            }

            if (primaryUseKey.IsReleased)
            {
                var screenPos = Input.mousePosition;
                var worldPos = _blockRaycaster.GetWorldPosition(screenPos);

                if (_isPrimaryUseHeld)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        GameEventBus.Publish(new UIPrimaryUseEnded(screenPos));
                        if (_primaryUseHoldTime < 0.2f)
                            GameEventBus.Publish(new UIPrimaryUseClicked(screenPos));
                    }
                    else
                    {
                        GameEventBus.Publish(new PrimaryUseEnded(screenPos, worldPos));
                        if (_primaryUseHoldTime < 0.2f)
                            GameEventBus.Publish(new PrimaryUseClicked(screenPos, worldPos));
                    }
                }

                _isPrimaryUseHeld = false;
            }


            if (InputBindingManager.Bindings.GetBinding(InputAction.SecondaryUse).IsReleased &&
                !EventSystem.current.IsPointerOverGameObject())
            {
                var screenPos = Input.mousePosition;
                var worldPos = _blockRaycaster.GetWorldPosition(screenPos);
                GameEventBus.Publish(new SecondaryUseRequested(screenPos, worldPos));
            }
            
            float scroll = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(0f, scroll))
            {
                GameEventBus.Publish(new MouseScrolledEvent(scroll));
            }
                
        }
        
        private float _lastSlotSelectTime = -1f;
        private void HandleInventorySlotInput(bool isUIBlocking)
        {
            if (Time.time - _lastSlotSelectTime < SlotSelectCooldown)
                return;
            
            for (InputAction slot = InputAction.SlotStart; slot <= InputAction.SlotEnd; slot++)
            {
                var binding = InputBindingManager.Bindings.GetBinding(slot);
                if (binding.IsPressed)
                {
                    int index = slot - InputAction.SlotStart;
                    if (isUIBlocking)
                    {
                        GameEventBus.Publish(new ItemSlotKeyPressedEvent(_player, index, SlotCollectionType.Inventory));
                    }
                    else
                    {
                        GameEventBus.Publish(new SelectedSlotChangeRequest(index, _player));
                    }
                    _lastSlotSelectTime = Time.time;
                    break;
                }
            }
            
            if (isUIBlocking)
                return;
            
            var scroll = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(0f, scroll))
            {
                var inventory = _player.InventoryManager.GetInventory<PlayerInventory>(SlotCollectionType.Inventory);
                int selected = inventory.SelectedSlotIndex;
                int sign = scroll > 0 ? -1 : 1;
                int count = Configs.GameConfig.Inventory.Player.HotbarSlotCount;

                int newSelected = (selected + sign + count) % count;
                GameEventBus.Publish(new SelectedSlotChangeRequest(newSelected, _player));
            }

        }
    }
}
