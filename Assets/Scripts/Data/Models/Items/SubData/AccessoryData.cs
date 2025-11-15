using System.Text;
using Config;
using Data.Models.Items.Tooltip;
using Generated.Localization;
using Localization;
using Systems.BuffSystem;
using Systems.StatSystem;

namespace Data.Models.Items.SubData
{
    public class AccessoryData : ITooltipProvider
    {
        public StatModifier[] Stats { get; set; }
        public EffectData[] Effects { get; set; }
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            sb.AppendLine(LocalizationDatabase.Get(LocalizationKeys.ItemPropertyAccessory));

            if (Stats != null && Stats.Length > 0)
                foreach (var stat in Stats)
                    sb.AppendLine(stat.Format(tooltipConfig));
            
            if (Effects != null && Effects.Length > 0)
                foreach (var effect in Effects)
                    sb.AppendLine(effect.GetEffectDescription());
        }
    }
}