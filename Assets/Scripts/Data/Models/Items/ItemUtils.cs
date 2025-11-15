using Generated.Ids;

namespace Data.Models.Items
{
    public static class ItemUtils
    {
        public static bool IsEmpty(string id)
            => string.IsNullOrEmpty(id) || id == ItemIds.Empty;
        
        public static bool IsSameTypeItem(this IItem item, IItem other)
            => item.ItemData.Id == other.ItemData.Id;   
    }
}