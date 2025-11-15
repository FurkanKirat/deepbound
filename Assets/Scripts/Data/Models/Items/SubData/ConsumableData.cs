using System;
using System.Text;
using Config;
using Data.Models.Items.Tooltip;
using Generated.Localization;
using Localization;

namespace Data.Models.Items.SubData
{
    [Serializable]
    public class ConsumableData : ITooltipProvider
    {
        public int HealAmount { get; set; }
        public float Cooldown { get; set; }
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            sb.AppendLine(LocalizationDatabase.Get(LocalizationKeys.ItemPropertyConsumable));
        }
    }
}