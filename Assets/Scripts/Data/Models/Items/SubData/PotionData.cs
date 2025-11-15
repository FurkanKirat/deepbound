using System.Text;
using Config;
using Data.Models.Items.Tooltip;
using Generated.Localization;
using Localization;
using Systems.BuffSystem;
using Systems.StatSystem;
using UnityEngine;

namespace Data.Models.Items.SubData
{
    public class PotionData : ITooltipProvider
    {
        public float? Duration { get; set; }
        public StatModifier[] Stats { get; set; }
        public EffectData[] Effects { get; set; }
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            sb.AppendLine(LocalizationDatabase.Get(LocalizationKeys.ItemPropertyDrinkable));

            if (Duration != null && !Mathf.Approximately(Duration.Value, 0f))
            {
                sb.AppendLine(LocalizationDatabase.Get(LocalizationKeys.ItemPropertyDuration) + $": {Duration:0.#}s");
            }

            if (Stats != null && Stats.Length > 0)
                foreach (var stat in Stats)
                    sb.AppendLine(stat.Format(tooltipConfig));
            
            if (Effects != null && Effects.Length > 0)
                foreach (var effect in Effects)
                    sb.AppendLine(effect.GetEffectDescription());
        }
    }
}