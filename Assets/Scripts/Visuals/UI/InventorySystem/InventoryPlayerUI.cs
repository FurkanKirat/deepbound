using Core;
using Core.Context;
using Core.Events;
using Data.Models.Items;
using Interfaces;
using Systems.CombatSystem.Armor;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals.UI.InventorySystem
{
    public class InventoryPlayerUI : MonoBehaviour, IInitializable<ClientContext>
    {
        [SerializeField] private Image playerImage, headImage, chestImage, pantsImage;
        private IPlayer _playerLogic;
        public void Initialize(ClientContext data)
        {
            _playerLogic ??= data.Player;
            playerImage.sprite = _playerLogic.Icon.Load();
        }

        private void OnEnable()
        {
            UpdateImages();
            GameEventBus.Subscribe<InventorySlotChangedEvent>(OnInventorySlotChanged);
            GameEventBus.Subscribe<InventoryChangedEvent>(OnInventoryChanged);
        }

        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<InventorySlotChangedEvent>(OnInventorySlotChanged);
            GameEventBus.Unsubscribe<InventoryChangedEvent>(OnInventoryChanged);
        }

        private void OnInventorySlotChanged(InventorySlotChangedEvent e)
        {
            if (e.SlotCollectionType != SlotCollectionType.Equipment && e.Owner.InventoryOwnerType != InventoryOwnerType.Player)
                return;
            
            UpdateImages();
        }

        
        private void OnInventoryChanged(InventoryChangedEvent e)
        {
            if (e.Inventory.Type != SlotCollectionType.Equipment && e.Owner.InventoryOwnerType != InventoryOwnerType.Player)
                return;
            
            UpdateImages();
        }
        
        private void UpdateImages()
        {
            var equipment = _playerLogic.InventoryManager.GetInventory<Equipment>(SlotCollectionType.Equipment);

            var head = equipment.GetItem(EquipmentSlot.Head).GetArmorData()?.Sprite?.Load();
            var chest = equipment.GetItem(EquipmentSlot.Chest).GetArmorData()?.Sprite?.Load();
            var pants = equipment.GetItem(EquipmentSlot.Pants).GetArmorData()?.Sprite?.Load();

            SetSprite(headImage, head);
            SetSprite(chestImage, chest);
            SetSprite(pantsImage, pants);
        }
        
        private static void SetSprite (Image image, Sprite sprite)
        {
            if (sprite != null)
            {
                image.sprite = sprite;
                image.enabled = true;
            }
            else
                image.enabled = false;
        }
    }
}