using System.Collections.Generic;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Input;

namespace Systems.InputSystem
{
    public class InputBindingMap : ISaveable<InputBindingsSaveData>
    {
        private readonly Dictionary<InputAction, ActionBinding> _bindings = new();

        private InputBindingMap(Dictionary<InputAction, ActionBinding> bindings)
        {
            foreach (var binding in bindings)
                _bindings.Add(binding.Key, binding.Value);
        }
        
        public static InputBindingMap Load(InputBindingsSaveData saveData)
        {
            var bindings = new Dictionary<InputAction, ActionBinding>(InputDefaults.Bindings);

            foreach (var (action, binding) in saveData.Bindings)
                bindings[action] = ActionBinding.Load(binding);

            return new InputBindingMap(bindings);
        }
        
        public static InputBindingMap Create()
            => new (InputDefaults.Bindings);
        
        public InputBindingsSaveData ToSaveData()
        {
            var saveBindings = new Dictionary<InputAction, ActionBindingSaveData>();

            foreach (var (action, binding) in _bindings)
            {
                saveBindings.Add(action, binding.ToSaveData());
            }
            
            var saveData = new InputBindingsSaveData
            {
                Bindings = saveBindings
            };
            return saveData;
        }
        
        public ActionBinding GetBinding(InputAction action)
        {
            return _bindings.GetValueOrDefault(action);
        }
        
        public void SetBinding(InputAction action, ActionBinding key)
        {
            _bindings[action] = key;
        }
        
        public void ResetToDefaults()
        {
            _bindings.Clear();
            foreach (var kvp in InputDefaults.Bindings)
                _bindings[kvp.Key] = kvp.Value;
        }
        
    }

}