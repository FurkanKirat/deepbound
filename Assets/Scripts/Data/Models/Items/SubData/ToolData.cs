using System;
using System.Text;
using Config;
using Data.Models.Items.Tooltip;
using Generated.Localization;
using Localization;

namespace Data.Models.Items.SubData
{
    [Serializable]
    public class ToolData : ITooltipProvider
    {
        public ToolType ToolType { get; set; } // "Pickaxe", "Axe", etc.
        public float Speed { get; set; }
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            sb.AppendLine($"{LocalizationDatabase.Get(LocalizationKeys.ItemPropertyTool)}: " +
                          $"{ToolType}\n" +
                          $"{LocalizationDatabase.Get(LocalizationKeys.ItemPropertySpeed)}: " +
                          $"{Speed}");
        }
    }

    public enum ToolType : byte
    {
        Pickaxe
    }
}