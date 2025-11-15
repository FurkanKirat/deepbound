namespace Data.Models.Items
{
    public class ItemModifier
    {
        public string Id;                    // "burn", "life_steal", "poison"
        public float Value;                  
        public ModifierTrigger Trigger;      
    }

}