using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Input;

namespace Systems.InputSystem
{
    public class ActionBinding : ISaveable<ActionBindingSaveData>
    {
        public InputBinding Primary { get; }
        public InputBinding Secondary { get; }

        private ActionBinding(InputBinding primary, InputBinding secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }

        public static ActionBinding Create(InputBinding primary, InputBinding secondary)
        {
            return new ActionBinding(primary, secondary);
        }
        
        public static ActionBinding Load(ActionBindingSaveData saveData)
        {
            var primary = InputBinding.Load(saveData.Primary);
            var secondary = InputBinding.Load(saveData.Secondary);
            return new ActionBinding(primary, secondary);
        }

        public bool IsPressed => 
            (Primary != null && Primary.IsPressed) || (Secondary != null && Secondary.IsPressed);

        public bool IsReleased =>
            (Primary != null && Primary.IsReleased) || (Secondary != null && Secondary.IsReleased);
        
        public bool IsHeld =>
            (Primary != null && Primary.IsHeld) || (Secondary != null && Secondary.IsHeld);
        
        public ActionBindingSaveData ToSaveData()
        {
            return new ActionBindingSaveData
            {
                Primary = Primary?.ToSaveData(),
                Secondary = Secondary?.ToSaveData()
            };
        }
    }
}