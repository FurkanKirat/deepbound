using UnityEngine;

namespace Data.Models.Items
{
    public static class ItemAmountExtensions
    {
        public static Sprite GetSprite(this ItemAmount itemAmount)
            => itemAmount.ItemData.Icon.Load();
        
    }
}