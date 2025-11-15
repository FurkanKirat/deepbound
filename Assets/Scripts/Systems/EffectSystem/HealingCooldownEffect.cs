using System.Collections.Generic;
using Core.Context.Registry;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData;
using Utils.Extensions;

namespace Systems.EffectSystem
{
    public class HealingCooldownEffect : BaseEffect
    {
        public override string Id => EffectIds.HealingCooldown;
        private readonly float _cooldown;

        public HealingCooldownEffect(EffectContext context) : base(context)
        {
            _cooldown = context.EffectData.Parameters.Get("amount", 0f);
        }

        public HealingCooldownEffect(EffectContext context, EffectSaveData saveData) : base(context, saveData)
        {
            _cooldown = saveData.Parameters.Get("amount", 0f);
        }

        public override void OnApply(IEntity owner)
        {
            owner.AddCooldown(CooldownType.Healing, _cooldown);
        }
        public override EffectSaveData ToSaveData()
        {
            return new EffectSaveData
            {
                EffectId = Id,
                Duration = Duration,
                Parameters = new Dictionary<string, object>()
                {
                    ["amount"] = _cooldown
                }
            };
        }
    }
}