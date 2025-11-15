using Constants;
using Core;
using Data.Models.Items.SubData;
using Data.Models.References;
using Localization;
using Utils;            

namespace Data.Models.Items
{
    public class ItemData: IIdentifiable, IFallbackable
    {
        #region Serialized Keys
        public string Id { get; set; }
        public SpriteRef Icon { get; set; }
        public ItemBehaviorRef Behavior { get; set; }
        #endregion
        
        #region Serialized Properties
        public int MaxStack { get; set; }
        public float UseTime { get; set; } = 0.3f;
        public ItemCategory Category { get; set; }
        public string[] Tags { get; set; }
        #endregion
        
        #region Sub Data

        public ToolData ToolData { get; set; }
        public PlaceableData PlaceableData { get; set; }
        public ConsumableData ConsumableData { get; set; }
        public WeaponData WeaponData { get; set; }
        public ArmorData ArmorData { get; set; }
        public ItemCropData CropData { get; set; }
        public AccessoryData AccessoryData { get; set; }
        public PotionData PotionData { get; set; }

        #endregion

        #region Public Methods

        public bool HasTag(string tag) => Tags.ContainsItem(tag);
        public string GetName() => LocalizationDatabase.Get($"{Id}.name");
        public bool TryGetDescription(out string desc) 
            => LocalizationDatabase.TryGet($"{Id}.desc", out desc);
        
        #endregion
        
        public void ApplyFallbacks()
        {
            Icon ??= new SpriteRef(Defaults.ItemSprite);
        }
    }

}