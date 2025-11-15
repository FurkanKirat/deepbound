using System.Collections.Generic;
using System.Text;
using Data.Database;
using Data.Models.Items.Tooltip;
using Generated.Ids;
using Generated.Localization;
using Localization;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;

namespace Data.Models.Items
{
    public class ItemInstance : IItem, ISaveable<ItemSaveData>
    {
        public ItemData ItemData { get; }
        public int Count { get; set; }
        
        public List<ItemModifier> Modifiers { get; }
        
        private ItemInstance(string itemId, int amount, List<ItemModifier> modifiers)
        {
            ItemData = Databases.Items[itemId];
            Count = amount;
            Modifiers = modifiers;
        }
        
        public static ItemInstance Create(string itemId, int amount)
        {
            return new ItemInstance(itemId, amount, new List<ItemModifier>());
        }
        
        public static ItemInstance Load(ItemSaveData data)
        {
            if (data == null || data.ItemAmount.IsEmpty)
                return Empty;
            
            var id = data.ItemAmount.Id;
            var count = data.ItemAmount.Count;
            var modifiers = data.Modifiers != null
                ? new List<ItemModifier>(data.Modifiers)
                : new List<ItemModifier>();
            return new ItemInstance(id, count, modifiers);
        }
        
        private ItemInstance(ItemInstance other)
        {
            ItemData = other.ItemData;
            Count = other.Count;
            Modifiers = new List<ItemModifier>(other.Modifiers);
        }

        public static ItemInstance Empty => Create(ItemIds.Empty, 1);
        public bool IsEmpty => Count <= 0 || ItemUtils.IsEmpty(ItemData.Id);

        public ItemAmount ToItemAmount() => new (ItemData.Id, Count);

        public ItemSaveData ToSaveData() => new ItemSaveData
        {
            ItemAmount = new ItemAmount
            {
                Count = Count,
                Id = ItemData.Id
            },
            Modifiers = Modifiers
        };
        
        public ItemInstance Clone() => new ItemInstance(this);

        public ItemInstance CloneWithCount(int count)
        {
            var clone = Clone();
            clone.Count = count;
            return clone;
        }
        
        public string GetItemProperties()
        {
            var sb = new StringBuilder();
            if (Count > 1)
            {
                sb.AppendLine($"<b>{ItemData.GetName()}</b> ({Count})");
            }
            else
            {
                sb.AppendLine($"<b>{ItemData.GetName()}</b>");
            }
            
            foreach (var provider in GetTooltipProviders())
                provider.AppendTooltip(sb, Configs.TooltipConfig);
            
            if (ItemData.TryGetDescription(out var desc))
                sb.AppendLine(desc);
            
            return sb.ToString();
        }
        
        public IEnumerable<ITooltipProvider> GetTooltipProviders()
        {
            if (ItemData.ToolData != null) yield return ItemData.ToolData;
            if (ItemData.WeaponData != null) yield return ItemData.WeaponData;
            if (ItemData.ArmorData != null) yield return ItemData.ArmorData;
            if (ItemData.PlaceableData != null) yield return ItemData.PlaceableData;
            if (ItemData.ConsumableData != null) yield return ItemData.ConsumableData;
            if (ItemData.AccessoryData != null) yield return ItemData.AccessoryData;
            if (ItemData.CropData != null) yield return ItemData.CropData;
            if (ItemData.PotionData != null) yield return ItemData.PotionData;
        }
        
        public override string ToString()
        {
            return
                $"Type: {ItemData.Id}, {nameof(Count)}: {Count}, {nameof(Modifiers)}: {Modifiers}";
        }
    }


}