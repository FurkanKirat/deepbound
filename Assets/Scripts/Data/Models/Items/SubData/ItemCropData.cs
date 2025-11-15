using System.Text;
using Config;
using Data.Models.Items.Tooltip;

namespace Data.Models.Items.SubData
{
    public class ItemCropData : ITooltipProvider
    {
        public string CropId { get; set; }
        public string[] AllowedBaseBlocks { get; set; }
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            
        }
    }
}