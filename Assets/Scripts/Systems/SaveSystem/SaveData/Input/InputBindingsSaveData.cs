using System.Collections.Generic;
using Systems.InputSystem;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.Input
{
    public class InputBindingsSaveData : ISaveData
    {
        public Dictionary<InputAction, ActionBindingSaveData> Bindings { get; set; }
    }
}