using System.Collections.Generic;
using Core.Context.Registry;
using Systems.CombatSystem.Damage;
using Systems.CombatSystem.Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData;
using UnityEngine;
using Utils.Extensions;

namespace Systems.EffectSystem
{
    public class PoisonEffect : BaseEffect
    {
        public override string Id => EffectIds.Poison;
        public float Dps { get; }

        public PoisonEffect(EffectContext context) : base(context)
        {
            Dps = context.EffectData.Parameters.Get("dps", 0f);
        }

        public PoisonEffect(EffectContext context, EffectSaveData saveData) : base(context, saveData)
        {
            Dps = saveData.Parameters.Get("dps", 0f);
        }
        
        public override void Tick(IEntity owner, float deltaTime)
        {
            if (owner is IHasHealth hasHealth)
            {
                hasHealth.TakeDamage(new DamageInfo(
                    Dps * deltaTime,
                    DamageType.Poison,
                    null,
                    Vector2.zero
                    ));
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
                    ["dps"] = Dps,
                }
            };
        }
    }
}