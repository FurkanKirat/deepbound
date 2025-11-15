using System;

namespace Visuals.UI.Settings
{
    [Flags]
    public enum SettingsScope
    {
        None = 0,
        MainMenu = 1 << 0,
        Game = 1 << 1,
    }
}