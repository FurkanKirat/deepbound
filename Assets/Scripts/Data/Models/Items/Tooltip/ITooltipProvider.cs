using System.Text;
using Config;

namespace Data.Models.Items.Tooltip
{
    public interface ITooltipProvider
    {
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig);
    }
}