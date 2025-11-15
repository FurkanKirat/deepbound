using System.Collections.Generic;
using Systems.CombatSystem.Damage;
using UnityEngine;

namespace Systems.CombatSystem.Armor
{
    public class ArmorProfile
    {
        public float FlatArmor = 0f;
        
        private readonly Dictionary<DamageType, float> _typeArmors = new();
        
        public float ApplyArmor(DamageInfo info, float baseDamage)
        {
            float typeArmor = _typeArmors.GetValueOrDefault(info.Type, FlatArmor);
            
            float final = Mathf.Max(baseDamage - typeArmor, 0f);
            
            if (info.ArmorPenetration > 0)
            {
                float effectiveArmor = Mathf.Max(typeArmor - info.ArmorPenetration, 0f);
                final = Mathf.Max(baseDamage - effectiveArmor, 0f);
            }

            return final;
        }

        public void SetArmor(DamageType type, float value)
        {
            _typeArmors[type] = value;
        }

        public float GetArmor(DamageType type)
        {
            return _typeArmors.GetValueOrDefault(type, FlatArmor);
        }
    }

}