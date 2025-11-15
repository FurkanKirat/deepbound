using System.Collections.Generic;
using Systems.SaveSystem.Interfaces;

namespace Settings
{
    public class SettingsSaveData : ISaveData
    {
        public Dictionary<string, object> Settings { get; set; }
    }
}