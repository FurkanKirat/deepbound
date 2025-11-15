using System;
using System.Text;
using Config;
using Data.Models.Items.Tooltip;
using Generated.Localization;
using Localization;
using Systems.CombatSystem.Damage;

namespace Data.Models.Items.SubData
{
    [Serializable]
    public class WeaponData : ITooltipProvider
    {
        public int Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float Range { get; set; } = 1.5f;
        public DamageType DamageType { get; set; }
        public TrailData Trail { get; set; }
        public void AppendTooltip(StringBuilder sb, TooltipConfig tooltipConfig)
        {
            sb.AppendLine($"{LocalizationDatabase.Get(LocalizationKeys.ItemPropertyDamage)}: {Damage}");
        }
    }
}