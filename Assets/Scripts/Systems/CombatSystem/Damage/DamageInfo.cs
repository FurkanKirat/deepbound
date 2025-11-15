using Systems.EntitySystem.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Systems.CombatSystem.Damage
{
    public class DamageInfo
    {
        public float Amount { get; }
        public DamageType Type { get; }
        public IEntity Source { get; }
        public Vector2 Knockback { get; }
        public bool IsCritical { get; }
        public float ArmorPenetration { get; }

        public DamageInfo(
            float flatAmount,
            DamageType type,
            IEntity source,
            Vector2 knockback,
            Random random = null,
            float critRate = 0f,
            float critMultiplier = 2f,
            float armorPenetration = 0f)
        {
            Type = type;
            Source = source;
            Knockback = knockback;
            ArmorPenetration = armorPenetration;

            if (random != null && random.NextDouble() < critRate)
            {
                IsCritical = true;
                Amount = flatAmount * critMultiplier;
            }
            else
            {
                IsCritical = false;
                Amount = flatAmount;
            }
        }

    }

}