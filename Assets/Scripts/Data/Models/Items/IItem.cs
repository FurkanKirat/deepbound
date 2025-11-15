using UnityEngine;

namespace Data.Models.Items
{
    public interface IItem
    {
        ItemData ItemData { get; }
        int Count { get; }
        bool IsEmpty { get; }
    }

    public static class ItemExtensions
    {
        public static Sprite GetSprite(this IItem item)
            => item.ItemData.Icon.Load();
        
        public static string FormatItemCount(this IItem item) 
            => item.Count == 1 ? "" : item.Count.ToString();
        
    }
}