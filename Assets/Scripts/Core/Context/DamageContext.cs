namespace Core.Context
{
    public class DamageContext
    {
        public float Amount { get; }
        public float CritRate { get; }
        public float CritDamage { get; }
        
        public DamageContext(float amount, float critRate, float critDamage)
        {
            Amount = amount;
            CritRate = critRate;
            CritDamage = critDamage;
        }
    }
}