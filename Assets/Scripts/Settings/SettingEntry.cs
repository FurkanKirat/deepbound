namespace Settings
{
    public class SettingEntry
    {
        public string Key;
        public SettingType Type;
        public string[] Options;
        public string[] OptionKeys;
        public bool Localize;
        public float? Min;
        public float? Max;
        public float? Step;
        public object Default;
    }
}