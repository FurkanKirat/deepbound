using Core;
using Core.Events;
using Data.Database;
using Data.Models.Items;
using Systems.InventorySystem;
using UnityEngine;
using Utils;

namespace Visuals.ObjectVisuals.Player
{
    public class HandItemVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer itemRenderer;
        [SerializeField] private float idleAngle = -30f;

        public void SetFacing(bool facingRight)
        {
            if (facingRight)
            {
                transform.localPosition = Configs.PlayerConfig.HandOffset.ToVector3();
                itemRenderer.flipX = false;
                transform.localRotation = Quaternion.Euler(0, 0, idleAngle);
            }
            else
            {
                transform.localPosition = Configs.PlayerConfig.HandOffset.XNegated.ToVector3();
                itemRenderer.flipX = true;
                transform.localRotation = Quaternion.Euler(0, 0, -idleAngle);
            }
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<PlayerSpawnEvent>(OnPlayerSpawn);
            GameEventBus.Subscribe<SelectedSlotChangedEvent>(OnSelectedSlotChanged);
            GameEventBus.Subscribe<InventorySlotChangedEvent>(OnInventorySlotChanged);

        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<PlayerSpawnEvent>(OnPlayerSpawn);
            GameEventBus.Unsubscribe<SelectedSlotChangedEvent>(OnSelectedSlotChanged);
            GameEventBus.Unsubscribe<InventorySlotChangedEvent>(OnInventorySlotChanged);

        }
        
        private void OnInventorySlotChanged(InventorySlotChangedEvent e)
        {
            var player = e.Owner;
            if (player.InventoryOwnerType != InventoryOwnerType.Player)
                return;
            
            var inv = player.InventoryManager.GetPlayerInventory();
            if (e.SlotIndex != inv.SelectedSlotIndex)
                return;
            
            ChangeSprite(inv);
        }
        
        private void OnSelectedSlotChanged(SelectedSlotChangedEvent e)
        {
            ChangeSprite(e.Owner.InventoryManager.GetPlayerInventory());
        }

        private void OnPlayerSpawn(PlayerSpawnEvent e)
        {
            ChangeSprite(e.Player.InventoryManager.GetPlayerInventory());
            SetFacing(true);
        }

        private void ChangeSprite(Inventory inventory)
        {
            if (inventory == null)
            {
                GameLogger.Log("Inventory is null");
                return;
            }
            var item = inventory.GetSelectedItem();
            if (item != null)
                itemRenderer.sprite = item.GetSprite();
        }
        
        
    }
}