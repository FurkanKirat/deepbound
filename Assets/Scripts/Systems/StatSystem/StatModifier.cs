using Config;
using Generated.Localization;
using Localization;
using UnityEngine;
using Utils;

namespace Systems.StatSystem
{
    public class StatModifier
    {
        public StatType Type { get; }
        public float Value { get; }
        public StatOperation Operation { get; }

        public StatModifier(StatType type, float value, StatOperation operation)
        {
            Type = type;
            Value = value;
            Operation = operation;
        }

        public string Format(TooltipConfig tooltipConfig)
        {
            string typeName = LocalizationDatabase.Get($"stat_type.{Type.ToJsonString()}");

            switch (Operation)
            {
                case StatOperation.Add:
                    if (Mathf.Approximately(Value, 0f)) return "";
                    string colorAdd = Value >= 0 ? tooltipConfig.PositiveColor : tooltipConfig.NegativeColor;
                    return $"<color={colorAdd}>{(Value >= 0 ? "+" : "")}{Value:0.#} {typeName}</color>";

                case StatOperation.Multiply:
                    float percent = (Value - 1f) * 100f;
                    if (Mathf.Approximately(percent, 0f)) return "";
                    string colorMult = percent > 0 ? tooltipConfig.PositiveColor : tooltipConfig.NegativeColor;
                    return $"<color={colorMult}>{(percent > 0 ? "+" : "-")}{Mathf.Abs(percent):0.#}% {typeName}</color>";

                case StatOperation.Override:
                    return $"<color={tooltipConfig.NeutralColor}>{string.Format(LocalizationDatabase.Get(LocalizationKeys.StatOperationOverride), typeName, Value.ToString("0.#"))}</color>";

                default:
                    return "";
            }
        }

    }
}