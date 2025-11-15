using System.Text;
using Config;
using Data.Models.Items.Tooltip;
using Generated.Localization;
using Localization;
using Newtonsoft.Json;

namespace Data.Models.Items.SubData
{
 
    public class PlaceableData : ITooltipProvider
    {
        [JsonProperty] 
        public string BlockId { get; set; }

        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            sb.AppendLine(LocalizationDatabase.Get(LocalizationKeys.ItemPropertyCanBePlaced));
        }
    }
}