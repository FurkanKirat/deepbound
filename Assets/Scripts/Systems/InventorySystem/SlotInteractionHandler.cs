using System;
using Core;
using Core.Events;
using Data.Models.Items;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Player;
using Visuals.UI;

namespace Systems.InventorySystem
{
    public class SlotInteractionHandler
    {
        private readonly IPlayer _player;
        private readonly HeldItemHandler _heldItemHandler;

        public SlotInteractionHandler(IPlayer player, HeldItemHandler heldItemHandler)
        {
            _player = player;
            _heldItemHandler = heldItemHandler;
            GameEventBus.Subscribe<ItemSlotClickedEvent>(OnSlotLeftClicked);
            GameEventBus.Subscribe<SwapSlotsRequest>(OnSwapSlotsRequest);
        }
        

        public void Dispose()
        {
            GameEventBus.Unsubscribe<ItemSlotClickedEvent>(OnSlotLeftClicked);
            GameEventBus.Unsubscribe<SwapSlotsRequest>(OnSwapSlotsRequest);
        }

        private void OnSlotLeftClicked(ItemSlotClickedEvent evt)
        {
            switch (evt.ClickType)
            {
                case SlotClickType.LeftClick:
                case SlotClickType.RightClick:
                    HandleSlotClick(evt.SlotIndex, evt.Owner.InventoryManager.GetInventory(evt.SlotCollectionType), evt.ClickType);
                    break;
                case SlotClickType.ShiftLeftClick:
                    QuickTransfer(evt.SlotIndex, evt.Owner.InventoryManager.GetInventory(evt.SlotCollectionType));
                    break;
                case SlotClickType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleSlotClick(int slotIndex, IItemSlotCollection collection, SlotClickType clickType)
        {
            var clickedItem = collection.GetItem(slotIndex);
            bool isSlotEmpty = collection.IsEmptyAt(slotIndex);
            bool isHandEmpty = _heldItemHandler.IsEmpty;

            switch (clickType)
            {
                case SlotClickType.LeftClick:
                    if (!isHandEmpty && !isSlotEmpty)
                    {
                        var amount = _heldItemHandler.HeldItem.ToItemAmount();
                        var tuple = collection.AcceptAtIfSame(slotIndex, amount);
                        if (tuple.accepted)
                        {
                            _heldItemHandler.UpdateRemaining(tuple.RemainingCount);
                        }
                        else
                        {
                            SwapItems(slotIndex, collection);
                        }
                    }
                    else if (!isHandEmpty)
                    {
                        if (collection.CanSetAt(slotIndex, _heldItemHandler.HeldItem))
                        {
                            collection.SetItemAt(slotIndex, _heldItemHandler.HeldItem);
                            _heldItemHandler.Clear();
                        }
                    }
                    else if (!isSlotEmpty)
                    {
                        _heldItemHandler.Set(clickedItem);
                        collection.ClearItemAt(slotIndex);
                    }
                    break;
                
                case SlotClickType.RightClick:
                    if (!isHandEmpty && !isSlotEmpty)
                    {
                        var amount = new ItemAmount(_heldItemHandler.HeldItem.ItemData.Id, 1);
                        var tuple = collection.AcceptAtIfSame(slotIndex, amount);
                        if (tuple.accepted)
                            _heldItemHandler.Decrease(1);
                        else
                            SwapItems(slotIndex, collection);
                    }
                    else if (!isHandEmpty)
                    {
                        if (collection.CanSetAt(slotIndex, _heldItemHandler.HeldItem))
                        {
                            var slotItem = _heldItemHandler.HeldItem.CloneWithCount(1);
                            collection.SetItemAt(slotIndex, slotItem);
                            _heldItemHandler.Decrease(1);
                        }
                    }
                    else if (!isSlotEmpty)
                    {
                        int slotCount = clickedItem.Count;
                        int newSlotCount = slotCount / 2;
                        int heldCount = slotCount - newSlotCount;
                        
                        if (heldCount > 0)
                        {
                            _heldItemHandler.Set(clickedItem.CloneWithCount(heldCount));
                            collection.ChangeItemCountAt(slotIndex, newSlotCount);
                        }
                    }
                    break;
            }

            GameEventBus.Publish(new HeldItemChangedEvent(_player, _heldItemHandler.HeldItem));
        }

        private void SwapItems(int slotIndex, IItemSlotCollection collection)
        {
            var handItem = _heldItemHandler.HeldItem; 
            var slotItem = collection.GetItem(slotIndex);
            if (collection.CanSetAt(slotIndex, handItem))
            {
                _heldItemHandler.Set(slotItem);
                collection.SetItemAt(slotIndex, handItem);
            }
        }


        private void QuickTransfer(int slotIndex, IItemSlotCollection collection)
        {
            var item = collection.GetItem(slotIndex);
            if (item.IsEmpty)
                return;
            
            var clone = item.Clone();
            
            var panels = UIPanelManager.Instance.InventoryRootPanelController.GetShiftPriority();
            foreach (var panel in panels)
            {
                if (panel.Inventory == collection)
                    continue;
                
                if (panel.Inventory.AcceptItem(clone))
                {
                    collection.ClearItemAt(slotIndex);
                    return;
                }
            }
            
            collection.ClearItemAt(slotIndex);
            collection.AcceptItem(clone);
        }
        
        
        private void OnSwapSlotsRequest(SwapSlotsRequest e)
        {
            var inv1 = e.SlotCollection1;
            var inv2 = e.SlotCollection2;
            var index1 = e.Index1;
            var index2 = e.Index2;
            
            var item1 = inv1.GetItem(index1);
            var item2 = inv2.GetItem(index2);
            if (!inv1.CanSetAt(index1, item2) || !inv2.CanSetAt(index2, item1))
                return;
            
            inv1.SetItemAt(index1, item2);
            inv2.SetItemAt(index2, item1);
        }


    }
}