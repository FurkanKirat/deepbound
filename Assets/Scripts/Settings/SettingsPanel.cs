using Visuals.UI.Settings;

namespace Settings
{
    public class SettingsPanel
    {
        public string Id;
        public string DisplayNameKey;
        public SettingsScope Scope = SettingsScope.MainMenu | SettingsScope.Game;
        public SettingEntry[] Settings;
    }
}