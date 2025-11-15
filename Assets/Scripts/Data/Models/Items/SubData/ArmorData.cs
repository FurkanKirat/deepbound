using System.Text;
using Config;
using Data.Models.Items.Tooltip;
using Data.Models.References;
using Generated.Localization;
using Localization;
using Systems.BuffSystem;
using Systems.CombatSystem.Armor;
using Systems.StatSystem;

namespace Data.Models.Items.SubData
{
    public class ArmorData: ITooltipProvider
    {
        public EquipmentSlot Slot;
        public StatModifier[] Stats;
        public EffectData[] Effects;
        public SpriteRef Sprite;
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            sb.AppendLine(LocalizationDatabase.Get(LocalizationKeys.ItemPropertyEquipable));
            if (Stats != null && Stats.Length > 0)
                foreach (var stat in Stats)
                    sb.AppendLine(stat.Format(tooltipConfig));
            
            if (Effects != null && Effects.Length > 0)
                foreach (var effect in Effects)
                    sb.AppendLine(effect.GetEffectDescription());
        }
    }
}