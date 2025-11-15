using System;
using System.Collections.Generic;
using Core;
using Core.Events;
using Data.Models.Crafting;
using Data.Models.Items;
using Interfaces;
using Systems.SaveSystem.SaveData.Inventory;
using Utils;

namespace Systems.InventorySystem
{
    public class Inventory : 
        IItemSlotCollection,
        IDisposable, 
        IInitializable
    {
        #region Fields
        protected readonly SlotStorage SlotStorage;
        
        protected readonly IInventoryOwner Owner;
        #endregion
        
        #region Properties

        public SlotCollectionType Type => SlotCollectionType.Inventory;
        public int SelectedSlotIndex { get; private set; } = -1;

        public IEnumerable<ItemInstance> AllItems
        {
            get
            {
                for (var i = 0; i < SlotCount; i++)
                    yield return SlotStorage.Slots[i].Item;
            }
        }
        #endregion
        
        #region Constructor & Dispose
        public Inventory(int size, IInventoryOwner owner)
        {
            SlotStorage = new SlotStorage(size);
            Owner = owner;
        }
        
        public Inventory(InventorySaveData data, IInventoryOwner owner)
        {
            Owner = owner;
            SlotStorage = new SlotStorage(data.SlotStorage);
            SelectedSlotIndex = data.SelectedSlot;
        }

        public virtual void Initialize()
        {
            GameEventBus.Publish(new InventoryChangedEvent(Owner, this));
        }
        
        public virtual void Dispose()
        {
            SlotStorage.ClearSlots();
        }
        #endregion

        #region Slot Selection
        protected void SelectSlot(int index)
        {
            if (!SlotStorage.HasSlot(index)) return;
            var oldIndex = SelectedSlotIndex;
            SelectedSlotIndex = index;
            GameEventBus.Publish(new SelectedSlotChangedEvent(Owner, oldIndex,index));
        }
        
        public bool IsEmptyAt(int index) 
            => SlotStorage.IsEmptyAt(index);
        
        #endregion
        
        #region Item Getters & Setters
        public void SetItemAt(int slotIndex, ItemInstance itemInstance)
        {
            if (!SlotStorage.TryGetSlot(slotIndex, out var slot))
                return;
            
            SlotStorage.SetItemAt(slotIndex, itemInstance);
            GameEventBus.Publish(new InventorySlotChangedEvent(Owner, this, slotIndex, slot.Item));
        }

        public ItemInstance GetSelectedItem() => GetItem(SelectedSlotIndex);
        
        public ItemInstance GetItem(int index) => SlotStorage.GetItemAt(index);
       
        #endregion
        
        #region Item Count Modifiers

        public (bool accepted, int RemainingCount) AcceptAtIfSame(int index, ItemAmount amount)
        {
            if (!SlotStorage.TryGetSlot(index, out var slot))
                return (false, 0);
            
            int acceptCount = slot.AcceptCount(amount);
            if (acceptCount <= 0)
                return (false, 0);
            
            int slotCount = slot.Item.Count;
            int total = slotCount + amount.Count;
            int maxStack = slot.Item.ItemData.MaxStack;
            if (total > maxStack)
            {
                slot.SetCount(maxStack);
                GameEventBus.Publish(new InventorySlotChangedEvent(Owner, this, index, slot.Item));
                return (true, total - maxStack);
            }

            slot.SetCount(total);
            GameEventBus.Publish(new InventorySlotChangedEvent(Owner, this, index, slot.Item));
            return (true, 0);
        }

        public void ChangeItemCountAt(int index, int count)
        {
            if (!SlotStorage.TryGetSlot(index, out var slot) || slot.IsEmpty)
                return;
            
            if (count <= 0)
            {
                SlotStorage.ClearSlot(index);
                
                GameEventBus.Publish(new ItemRemovedEvent(Owner, slot.Item, slot.Item.Count));
            }
            else
            {
                SlotStorage.GetItemAt(index).Count = count;
            }
            GameEventBus.Publish(new InventorySlotChangedEvent(
                Owner,
                this,
                index,
                slot.Item
            ));
        }

        public void IncreaseItemCount(int index, int increaseCount)
        {
            if (!SlotStorage.TryGetSlot(index, out var slot))
                return;
            ChangeItemCountAt(index, slot.Item.Count + increaseCount);
        }

        public void DecreaseItemCount(int index, int decreaseCount)
        {
            if (!SlotStorage.TryGetSlot(index, out var slot))
                return;
            ChangeItemCountAt(index, slot.Item.Count - decreaseCount);
        }
        public void ClearItemAt(int index)
        {
            ChangeItemCountAt(index, 0);
        }
        #endregion
        
        #region Item Accept / Add
        public bool AcceptItem(ItemInstance item)
        {
            for (int phase = 0; phase < 2; phase++)
            {
                for (int i = 0; i < SlotStorage.SlotCount; i++)
                {
                    var slot = SlotStorage.GetSlot(i);
                    
                    // Phase 1: try stacking into existing slots
                    if(phase == 0 && slot.IsEmpty) continue;
                    
                    // Phase 2: try filling empty slots
                    if (phase == 1 && !slot.IsEmpty) continue;
                    
                    int acceptCount = slot.AcceptCount(item);
                    
                    if (acceptCount <= 0) continue;
                    //GameLogger.Log($"Accepting item: {item} with {acceptCount}");
                    slot.Accept(item.Clone(), acceptCount);
                    item.Count -= acceptCount;
                    GameEventBus.Publish(new InventorySlotChangedEvent(Owner, this, i, slot.Item));

                    if (item.Count == 0)
                        return true;
                }
                
            }
            GameLogger.Log("Inventory is full");
            return false;
        }

        public void AcceptAt(ItemInstance item, int index)
        {
            if (!SlotStorage.TryGetSlot(index, out var slot))
                return;
            slot.Accept(item.Clone(), item.Count);
            GameEventBus.Publish(new InventorySlotChangedEvent(Owner, this, index, slot.Item));
        }
        #endregion
        
        #region Inventory Checks
        public bool HasItem(string itemId, int amount)
        {
            var inventoryDict = ToInventoryDict();
            return inventoryDict.GetValueOrDefault(itemId) >= amount;
        }

        public bool HasItemWithTag(string tag, int amount = 1)
        {
            int total = 0;
            foreach (var slot in SlotStorage.Slots)
            {
                if (slot.IsEmpty || !slot.Item.HasTag(tag)) continue;
                
                total += slot.Item.Count; 
                
                if (total >= amount)
                    return true;
                
            }
            return total >= amount;
        }
        
        public bool CanCraft(CraftingRecipe recipe)
        {
            var inventoryDict = ToInventoryDict();
            
            foreach (var requiredItem in recipe.Requires)
            {
                if (!inventoryDict.TryGetValue(requiredItem.Id, out var itemCount) || itemCount < requiredItem.Count)
                {
                    return false;
                }
            }

            return true;
        }

        public Dictionary<CraftingRecipe, bool> CanCraft(IEnumerable<CraftingRecipe> recipes)
        {
            var inventoryDict = ToInventoryDict();
            var result = new Dictionary<CraftingRecipe, bool>();
            foreach (var recipe in recipes)
            {
                bool canCraft = true;
                foreach (var requiredItem in recipe.Requires)
                {
                    if (!inventoryDict.TryGetValue(requiredItem.Id, out var itemCount) || itemCount < requiredItem.Count)
                    {
                        canCraft = false;
                        break;
                    }
                
                }
                result[recipe] = canCraft;
            }
            
            return result;
        }
        #endregion
        
        #region Inventory Utility
        public Dictionary<string, int> ToInventoryDict()
        {
            var dict = new Dictionary<string, int>();

            for (int i = 0; i < SlotCount; i++)
            {
                var slot = SlotStorage.GetSlot(i);
                if (slot.IsEmpty) continue;

                var item = slot.Item;
                var key = item.ItemData.Id;
                
                if (dict.TryGetValue(key, out int count))
                    dict[key] = count + item.Count;
                else
                    dict[key] = item.Count;
            }

            return dict;
        }

        public void Shuffle(Random random)
        {
            SlotStorage.Shuffle(random);
            GameEventBus.Publish(new InventoryChangedEvent(Owner, this));
        }
        
        #endregion
        
        #region Item Removal
        
        public bool RemoveItem(string itemId, int count)
        {
            return RemoveItemWithCondition(count, slot => slot.Item.ItemData.IsSameAs(itemId));
        }
        public bool RemoveItemWithTag(string tag, int count)
        {
            return RemoveItemWithCondition(count, slot => slot.Item.ItemData.HasTag(tag));
        }
        
        private bool RemoveItemWithCondition(int count, Func<InventorySlot, bool> predicate)
        {
            var removedItems = new Dictionary<int, int>();

            int cumulative = 0;
            for (int i = SlotStorage.SlotCount - 1; i >= 0; i--)
            {
                var slot = SlotStorage.Slots[i];
                if (!slot.IsEmpty && predicate(slot))
                {
                    int slotCount = slot.Item.Count;
                    
                    int remaining = count - cumulative;
                    if (slotCount >= remaining)
                    {
                        removedItems.Add(i, remaining);
                        cumulative += remaining;
                        break;
                    }
                    else
                    {
                        removedItems.Add(i, slotCount);
                        cumulative += slotCount;
                    }
                    
                }
            }

            if (cumulative < count)
            {
                GameLogger.Log($"Cannot remove cumulative: {cumulative} < count: {count}");
                return false;
            }
                

            foreach (var (slot, removeCount) in removedItems)
            {
                DecreaseItemCount(slot, removeCount);
            }

            return true;
        }
        
        #endregion
        
        #region IItemSlotCollection

        public int SlotCount => SlotStorage.SlotCount;
        public bool CanSetAt(int index, ItemInstance item) => true;

        #endregion
        
        #region Serialization

        public SlotCollectionSaveData ToSaveData()
        {
            return new InventorySaveData
            {
                Type = Type,
                SlotStorage = SlotStorage.ToSaveData(),
                SelectedSlot = SelectedSlotIndex,
            };
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            var items = new List<string>();
            for (int i = 0; i < SlotCount; i++)
            {
                var slot = SlotStorage.GetSlot(i);
                if (!slot.IsEmpty)
                    items.Add($"[{i}] {slot.Item.ItemData.Id} x{slot.Item.Count}");
            }

            string selected = SelectedSlotIndex >= 0 && SelectedSlotIndex < SlotCount
                ? $"Selected: Slot {SelectedSlotIndex} ({GetItem(SelectedSlotIndex).GetBlockId() ?? "Empty"})"
                : "No slot selected";

            return $"Inventory ({SlotCount} slots) {selected}\n" + string.Join(", ", items);
        }


        #endregion

    }
}