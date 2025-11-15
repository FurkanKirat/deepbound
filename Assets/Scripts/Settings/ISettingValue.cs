using System;

namespace Settings
{
    public interface ISettingValue
    {
        object GetValue();
        void SetValue(object value);
        public event Action OnChanged;
    }
}