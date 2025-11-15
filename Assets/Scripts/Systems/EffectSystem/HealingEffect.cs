using System.Collections.Generic;
using Core.Context.Registry;
using Systems.CombatSystem.Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData;
using Utils.Extensions;

namespace Systems.EffectSystem
{
    public class HealingEffect : BaseEffect
    {
        public override string Id => EffectIds.Healing;
        private readonly float _amount;
        
        public HealingEffect(EffectContext context) : base(context)
        {
            _amount = context.EffectData.Parameters.Get("amount", 0f);
        }

        public HealingEffect(EffectContext context, EffectSaveData saveData) : base(context, saveData)
        {
            _amount = saveData.Parameters.Get("amount", 0f);
        }

        public override void OnApply(IEntity owner)
        {
            if (owner is IHasHealth hasHealth)
            {
                hasHealth.Heal(_amount);
            }
        }
        
        public override EffectSaveData ToSaveData()
        {
            return new EffectSaveData
            {
                EffectId = Id,
                Duration = Duration,
                Parameters = new Dictionary<string, object>
                {
                    ["amount"] = _amount
                }
            };
        }
    }
}