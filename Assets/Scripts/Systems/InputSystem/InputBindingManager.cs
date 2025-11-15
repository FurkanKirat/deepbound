using Constants.Paths;
using Systems.SaveSystem.SaveData.Input;
using Utils;

namespace Systems.InputSystem
{
    public static class InputBindingManager
    {
        private static bool _useDefaults = false;

        public static InputBindingMap Bindings { get; private set; }
        
        public static void Load()
        {
            if (!_useDefaults && FileUtils.FileExists(SaveConstants.BindingsFile))
            {
                var saveData = JsonHelper.LoadRaw<InputBindingsSaveData>(SaveConstants.BindingsFile);
                Bindings = InputBindingMap.Load(saveData);
            }
               
            else
            {
                Bindings = InputBindingMap.Create();
                Save();
            }
        }
        
        public static void Save()
        {
            JsonHelper.SaveRaw(SaveConstants.BindingsFile, Bindings.ToSaveData());
        }
    }
}