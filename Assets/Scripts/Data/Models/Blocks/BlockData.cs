using Constants;
using Core;
using Data.Models.Blocks.Subdata;
using Data.Models.Items;
using Data.Models.References;
using Data.Serializable;

namespace Data.Models.Blocks
{
    public class BlockData : IIdentifiable, IFallbackable
    {
        #region Identity
        public string Id { get; set; }
        #endregion

        #region Visuals
        public SpriteRef Icon { get; set; }
        public BlockAnimation IdleAnimation { get; set; }
        public ColorRef MapColor { get; set; }
        #endregion
        
        #region Sounds
        public AudioRef PlaceSound { get; set; }
        public AudioRef BreakSound { get; set; }
        public AudioRef InteractSound { get; set; }
        #endregion

        #region Behavior
        public BlockBehaviorRef[] Behaviors { get; set; }
        public string[] Tags { get; set; }
        
        public bool HasAnyBehavior => Behaviors is { Length: > 0 };
        #endregion

        #region Properties
        public float Hardness { get; set; }
        public bool IsSolid { get; set; }
        public bool IsDestructible { get; set; }
        public ItemAmount DropItem { get; set; }
        public Int2 Size { get; set; }
        
        public bool IsMultiple => Size.x > 1 || Size.y > 1;
        #endregion
        
        #region SubData
        public BlockCropData Crop { get; set; }
        public BlockStationData Station { get; set; }
        #endregion
        
        public void ApplyFallbacks()
        {
            Icon ??= new SpriteRef(Defaults.BlockSprite);
            MapColor ??= new ColorRef("FFFFFFFF");
            if (Size == Int2.Zero)
            {
                Size = Int2.One;
            }
            
            IdleAnimation ??= new BlockAnimation
            {
                Frames = new[] { Icon },
                Speed = 1f,
                Loop = true
            };
        }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Icon)}: {Icon}, {nameof(MapColor)}: {MapColor}, {nameof(PlaceSound)}: {PlaceSound}, {nameof(BreakSound)}: {BreakSound}, {nameof(Behaviors)}: {Behaviors}, {nameof(Tags)}: {Tags}, {nameof(HasAnyBehavior)}: {HasAnyBehavior}, {nameof(Hardness)}: {Hardness}, {nameof(IsSolid)}: {IsSolid}, {nameof(IsDestructible)}: {IsDestructible}, {nameof(DropItem)}: {DropItem}, {nameof(Size)}: {Size}, {nameof(IsMultiple)}: {IsMultiple}, {nameof(Crop)}: {Crop}";
        }
    }
}