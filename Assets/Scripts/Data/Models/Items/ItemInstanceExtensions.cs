using Data.Models.Items.SubData;
using Systems.CombatSystem.Armor;
using Systems.CombatSystem.Damage;
using UnityEngine;

namespace Data.Models.Items
{
    public static class ItemInstanceExtensions
    {
        #region Tags

        public static bool HasTag(this ItemInstance item, string tag)
            => item?.ItemData?.HasTag(tag) ?? false;
        #endregion
        
        #region Sprite

        public static Sprite GetSprite(this ItemInstance item)
            => item?.ItemData.Icon.Load();

        public static string GetSpriteKey(this ItemInstance item)
            => item?.ItemData.Icon.Key;
        #endregion
        
        #region Tool

        public static bool IsTool(this ItemInstance item)
            => item?.ItemData?.ToolData != null;

        public static ToolType? GetToolType(this ItemInstance item)
            => item?.ItemData?.ToolData?.ToolType;

        public static float GetToolSpeed(this ItemInstance item)
            => item?.ItemData?.ToolData?.Speed ?? 0f;

        #endregion

        #region Placeable

        public static bool IsPlaceable(this ItemInstance item)
            => item?.ItemData?.PlaceableData != null;

        public static string GetBlockId(this ItemInstance item)
            => item?.ItemData?.PlaceableData?.BlockId;

        #endregion

        #region Consumable

        public static bool IsConsumable(this ItemInstance item)
            => item?.ItemData?.ConsumableData != null;

        public static int GetHealAmount(this ItemInstance item)
            => item?.ItemData?.ConsumableData?.HealAmount ?? 0;

        public static float GetCooldown(this ItemInstance item)
            => item?.ItemData?.ConsumableData?.Cooldown ?? 0f;

        #endregion

        #region Weapon

        public static bool IsWeapon(this ItemInstance item)
            => item?.ItemData?.WeaponData != null;

        public static int GetDamage(this ItemInstance item)
            => item?.ItemData?.WeaponData?.Damage ?? 0;

        public static float GetAttackSpeed(this ItemInstance item)
            => item?.ItemData?.WeaponData?.AttackSpeed ?? 0f;

        public static float GetWeaponRange(this ItemInstance item)
            => item?.ItemData?.WeaponData?.Range ?? 1.5f;

        public static DamageType GetDamageType(this ItemInstance item)
            => item?.ItemData.WeaponData.DamageType ?? DamageType.Physical;

        #endregion

        #region Equipment

        public static bool IsEquippable(this ItemInstance item)
            => item?.ItemData.Category == ItemCategory.Armor &&
               item.GetArmorData() != null;

        public static EquipmentSlot? GetEquipmentSlot(this ItemInstance item)
            => item.GetArmorData()?.Slot;
        
        public static ArmorData GetArmorData(this ItemInstance item)
            => item?.ItemData?.ArmorData;

        #endregion
        
        #region Accessory
        public static bool IsAccessory(this ItemInstance item)
            => item?.ItemData.Category == ItemCategory.Accessory &&
               item.ItemData.AccessoryData != null;
        
        public static AccessoryData GetAccessoryData(this ItemInstance item)
            => item?.ItemData?.AccessoryData;
        #endregion

        #region Crop

        public static bool IsCrop(this ItemInstance item)
            => item?.ItemData?.CropData != null;

        public static ItemCropData CropData(this ItemInstance item)
            => item?.ItemData?.CropData;
        
        public static string CropId(this ItemInstance item)
            => CropData(item)?.CropId;
        #endregion

        #region Crosshair

        public static bool IsCrosshairOpen(this ItemInstance item)
        {
            return item.IsTool() || item.IsPlaceable();
        }

        #endregion
    }
}
