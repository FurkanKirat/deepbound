using System.Collections.Generic;
using Core;
using Core.Events;
using Localization;
using Systems.InventorySystem;
using TMPro;
using UnityEngine;
using Utils;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.InventorySystem.Panels
{
    public abstract class BaseInventoryPanel<TSlot> : 
        MonoBehaviour,
        ILocalizable,
        IInventoryPanel
        where TSlot : IItemSlotUI
    {
        protected IInventoryOwner InventoryOwner;

        protected readonly List<TSlot> Slots = new();
        private bool _isInitialized = false;
        

        public GameObject Root => root;
        [SerializeField] private GameObject root;
        protected abstract int SlotCount { get; }
        protected abstract int SlotOffset { get; }
        protected abstract UIInventoryType UIInventoryType { get; }
        protected abstract SlotCollectionType SlotCollectionType { get; }
        public abstract string OpenSound { get; }
        public abstract string CloseSound { get; }
        public abstract PanelType PanelType { get; }

        [SerializeField] private TMP_Text titleText;

        protected virtual void OnEnable()
        {
            GameEventBus.Subscribe<InventoryChangedEvent>(OnInventoryChanged);
            GameEventBus.Subscribe<InventorySlotChangedEvent>(OnInventorySlotChanged);
            GameEventBus.Subscribe<SelectedSlotChangedEvent>(OnSelectedSlotChanged);
            
            if (_isInitialized)
                UpdateUI();
        }

        protected virtual void OnDisable()
        {
            GameEventBus.Unsubscribe<InventoryChangedEvent>(OnInventoryChanged);
            GameEventBus.Unsubscribe<InventorySlotChangedEvent>(OnInventorySlotChanged);
            GameEventBus.Unsubscribe<SelectedSlotChangedEvent>(OnSelectedSlotChanged);
        }

        public virtual void Initialize(IInventoryOwner owner)
        {
            if (_isInitialized)
            {
                InventoryOwner = owner;
                UpdateUI();
                return;
            }
            
            InventoryOwner = owner;
            _isInitialized = true;   
            GenerateSlots();
            UpdateUI();
        }

        protected abstract void GenerateSlots();

        protected virtual void UpdateUI()
        {
            if (InventoryOwner == null) return;
            var collection = InventoryOwner.InventoryManager.GetInventory(SlotCollectionType);
            
            int selectedIndex = -1;
            if (collection is Inventory inventory)
            {
                selectedIndex = inventory.SelectedSlotIndex;
            }

            for (int i = 0; i < Slots.Count; i++)
            {
                int index = i + SlotOffset;
                var slot = Slots[i];
                slot.UpdateSlot(collection.GetItem(index));
                slot.UpdateSelection(index == selectedIndex);
            }

            Localize();
        }

        private void OnInventoryChanged(InventoryChangedEvent evt)
        {
            if (evt.Owner != InventoryOwner && SlotCollectionType != evt.Inventory.Type) return;
            UpdateUI();
        }

        private void OnInventorySlotChanged(InventorySlotChangedEvent evt)
        {
            if (evt.Owner != InventoryOwner || SlotCollectionType != evt.SlotCollectionType) return;
            int index = evt.SlotIndex - SlotOffset;
            if (Slots.TryGetItem(index, out var slot))
                slot.UpdateSlot(evt.SlotItem);
        }

        private void OnSelectedSlotChanged(SelectedSlotChangedEvent evt)
        {
            if (evt.Owner != InventoryOwner) return;

            int oldIndex = evt.OldIndex - SlotOffset;
            int newIndex = evt.NewIndex - SlotOffset;
            
            if (oldIndex == newIndex) return;

            if (Slots.IsInsideBounds(oldIndex, newIndex))
            {
                Slots[oldIndex].UpdateSelection(false);
                Slots[newIndex].UpdateSelection(true);
            }
        }
        
        public IItemSlotCollection Inventory => InventoryOwner.InventoryManager.GetInventory(SlotCollectionType);
        public void Localize()
        {
            if (titleText != null && InventoryOwner != null)
            {
                var ownerType = InventoryOwner.InventoryOwnerType;
                var titleKey = "inventory." + 
                               (ownerType == InventoryOwnerType.Player ? 
                                   SlotCollectionType.ToJsonString() : ownerType.ToJsonString());

                titleText.text = LocalizationDatabase.Get(titleKey);
            }
        }
    }

}