using Data.Models.Items;
using Utils;

namespace Systems.InventorySystem
{
    public class InventorySlot
    {
        public ItemInstance Item;

        public InventorySlot()
        {
            Item = ItemInstance.Empty;
        }
        public InventorySlot(InventorySlot other)
        {
            Item = other.Item.Clone();
        }
        public bool IsEmpty => Item == null || Item.IsEmpty;

        public bool CanAccept(ItemInstance comingItem)
        {
            return AcceptCount(comingItem) > 0;
        }
        
        public int AcceptCount(IItem comingItem)
        {
            // If slot is empty accept count is min(itemCount, itemMaxStack)
            if (IsEmpty) 
                return MathUtils.Min(comingItem.Count, comingItem.ItemData.MaxStack);
            
            // If item is not same type count is 0
            if (!Item.IsSameTypeItem(comingItem))
            {
                return 0;
            } 
            
            var maxStack = Item.ItemData.MaxStack;
            var slotCount = Item.Count;
            var comingItemCount = comingItem.Count;

            // If item is same type, accept as much as coming count fits
            return MathUtils.Min(comingItemCount, maxStack - slotCount);
        }
        
        public void Accept(ItemInstance comingItem, int count)
        {
            if (comingItem == null)
            {
                Clear();
                return;
            }
            
            if (IsEmpty)
            {
                Item = comingItem.Clone();
                Item.Count = count;
            }
            else
            {
                Item.Count += count;
            }
            
        }

        public void SetCount(int count)
        {
            Item.Count = count;
        }
        
        public void Swap(InventorySlot other)
        {
            (Item, other.Item) = (other.Item, Item);
        }
        
        public void Clear() => Item = ItemInstance.Empty;

        public override string ToString()
        {
            return $"{nameof(Item)}: {Item}, {nameof(IsEmpty)}: {IsEmpty}";
        }
    }
}