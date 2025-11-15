using System.Collections.Generic;
using Systems.InputSystem;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.Input
{
    public class InputBindingSaveData : ISaveData
    {
        public InputDevice Device { get; set; }
        public List<string> Keys { get; set; }
    }
}