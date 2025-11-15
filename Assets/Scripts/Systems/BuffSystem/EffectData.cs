using System.Collections.Generic;
using Core;
using Localization;
using Systems.EffectSystem;

namespace Systems.BuffSystem
{
    public class EffectData : IIdentifiable
    {
        public string Id { get; set; }
        public float? Duration { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        
        public string GetEffectDescription()
        {
            string effectDescription = LocalizationDatabase.Get($"effect.{Id}.desc");
            switch(Id)
            {
                case EffectIds.Healing:
                    if (Parameters.TryGetValue("amount", out var healAmount))
                        return string.Format(effectDescription, healAmount);
                    break;

                case EffectIds.Poison:
                    if (Parameters.TryGetValue("dps", out var dps))
                        return string.Format(effectDescription, dps);
                    break;

                case EffectIds.HealingCooldown:
                    if (Parameters.TryGetValue("amount", out var cooldown))
                        return string.Format(effectDescription, cooldown);
                    break;
            }

            return "";
        }

    }
}