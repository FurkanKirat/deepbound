using Data.Models.Items;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using UnityEngine;
using Visuals.UI.InventorySystem;

namespace Core.Events
{
    // General inventory change
    public readonly struct InventoryChangedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly IItemSlotCollection Inventory;
        
        public InventoryChangedEvent(IInventoryOwner owner, IItemSlotCollection inventory)
        {
            Owner = owner;
            Inventory = inventory;
        }
    }

    // A specific slot has changed
    public readonly struct InventorySlotChangedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly int SlotIndex;
        public readonly ItemInstance SlotItem;
        public readonly IItemSlotCollection SlotCollection;
        public SlotCollectionType SlotCollectionType => SlotCollection.Type;

        public InventorySlotChangedEvent(IInventoryOwner owner, IItemSlotCollection slotCollection, int slotIndex, ItemInstance slotItem)
        {
            Owner = owner;
            SlotCollection = slotCollection;
            SlotIndex = slotIndex;
            SlotItem = slotItem;
        }
    }

    // New item added (e.g. loot, after pickup)
    public readonly struct ItemAddedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly ItemInstance Item;
        public readonly int Amount;

        public ItemAddedEvent(IInventoryOwner owner, ItemInstance item, int amount)
        {
            Owner = owner;
            Item = item;
            Amount = amount;
        }
    }

    // Item used (drink potion, cast spell etc.)
    public readonly struct ItemUsedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly ItemInstance Item;

        public ItemUsedEvent(IInventoryOwner owner, ItemInstance item)
        {
            Owner = owner;
            Item = item;
        }
    }

    // Item removed (count became 0, dropped)
    public readonly struct ItemRemovedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly ItemInstance Item;
        public readonly int Amount;

        public ItemRemovedEvent(IInventoryOwner owner, ItemInstance item, int amount)
        {
            Owner = owner;
            Item = item;
            Amount = amount;
        }
    }

    // 6. Slot selection changed (only if player is using it)
    public readonly struct SelectedSlotChangedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly int OldIndex;
        public readonly int NewIndex;
        public ItemInstance SelectedItem => Owner.InventoryManager.GetInventory<Inventory>(SlotCollectionType.Inventory).GetSelectedItem();

        public SelectedSlotChangedEvent(IInventoryOwner owner, int oldIndex, int newIndex)
        {
            Owner = owner;
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
    }

    public readonly struct SelectedSlotChangeRequest : IEvent
    {
        public readonly int NewSlotIndex;
        public readonly IInventoryOwner Owner;

        public SelectedSlotChangeRequest(int newSlotIndex, IInventoryOwner owner)
        {
            NewSlotIndex = newSlotIndex;
            Owner = owner;
        }
    }
    
    public readonly struct HeldItemChangedEvent : IEvent
    {
        public readonly IPlayer Player;
        public readonly ItemInstance NewItem;

        public HeldItemChangedEvent(IPlayer player, ItemInstance newItem)
        {
            Player = player;
            NewItem = newItem;
        }
    }
    
    public readonly struct ItemSlotClickedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly int SlotIndex;
        public readonly UIInventoryType UIInventoryType;
        public readonly SlotCollectionType SlotCollectionType;
        public readonly SlotClickType ClickType;

        public ItemSlotClickedEvent(
            IInventoryOwner owner, 
            int slotIndex, 
            UIInventoryType uıInventoryType, 
            SlotCollectionType slotCollectionType,
            SlotClickType clickType)
        {
            Owner = owner;
            SlotIndex = slotIndex;
            UIInventoryType = uıInventoryType;
            SlotCollectionType = slotCollectionType;
            ClickType = clickType;
        }
    }
    
    public readonly struct ItemSlotHoveredEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly ItemInstance Item;
        public readonly Vector2 Position;

        public ItemSlotHoveredEvent(IInventoryOwner owner, ItemInstance item, Vector2 position)
        {
            Owner = owner;
            Item = item;
            Position = position;
        }
    }
    
    public readonly struct ItemSlotHoverEndedEvent : IEvent
    {
        
    }

    public readonly struct ItemSlotKeyPressedEvent : IEvent
    {
        public readonly IInventoryOwner Owner;
        public readonly int Index;
        public readonly SlotCollectionType SlotCollectionType;

        public ItemSlotKeyPressedEvent(IInventoryOwner owner, int index, SlotCollectionType slotCollectionType)
        {
            Owner = owner;
            Index = index;
            SlotCollectionType = slotCollectionType;
        }
    }

    public readonly struct SwapSlotsRequest : IEvent
    {
        public readonly IItemSlotCollection SlotCollection1;
        public readonly int Index1;
        public readonly IItemSlotCollection SlotCollection2;
        public readonly int Index2;

        public SwapSlotsRequest(IItemSlotCollection slotCollection1, int ındex1, IItemSlotCollection slotCollection2, int ındex2)
        {
            SlotCollection1 = slotCollection1;
            Index1 = ındex1;
            SlotCollection2 = slotCollection2;
            Index2 = ındex2;
        }
    }
}

