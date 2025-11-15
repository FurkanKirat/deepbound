using System.Collections.Generic;
using System.Linq;
using Core.Context;
using GameLoop;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;

namespace Systems.EffectSystem
{
    public class CooldownHandler : 
        ITickable,
        ISaveable<CooldownSaveData>
    {
        private readonly Dictionary<CooldownType, float> _cooldowns;

        private CooldownHandler(Dictionary<CooldownType, float> cooldowns)
        {
            _cooldowns = cooldowns;
        }

        public static CooldownHandler Create()
        {
            var cooldowns = new Dictionary<CooldownType, float>();
            return new CooldownHandler(cooldowns);
        }

        public static CooldownHandler Load(CooldownSaveData saveData)
        {
            return new CooldownHandler(saveData.Cooldowns);
        }
        
        public CooldownSaveData ToSaveData()
        {
            return new CooldownSaveData
            {
                Cooldowns = _cooldowns
            };
        }
        public void AddCooldown(CooldownType cooldown, float cooldownTime)
        {
            if (!_cooldowns.TryAdd(cooldown, cooldownTime))
            {
                _cooldowns[cooldown] += cooldownTime;
            }
        }
            
        public float GetCooldown(CooldownType key)
            => _cooldowns.GetValueOrDefault(key, 0);
        
        public bool HasCooldown(CooldownType key)
            => _cooldowns.TryGetValue(key, out var value) && value > 0;


        public void Tick(float timeInterval, TickContext ctx)
        {
            foreach (var (cooldown, cooldownTime) in _cooldowns.ToList())
            {
                var remaining = cooldownTime - timeInterval;
                if (remaining <= 0)
                    _cooldowns.Remove(cooldown);
                _cooldowns[cooldown] = remaining;
            }
        }
    }
}