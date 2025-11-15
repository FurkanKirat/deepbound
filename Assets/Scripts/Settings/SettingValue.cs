using System;
using System.Collections.Generic;

namespace Settings
{
    public class SettingValue<T> : ISettingValue
    {
        public T Value;
        public SettingValue(T initial) => Value = initial;
        public event Action<T> OnChangedValue;
        public event Action OnChanged;
        public object GetValue() => Value;
        public void SetValue(object value)
        {
            T newVal = (T)value;
            if (!EqualityComparer<T>.Default.Equals(Value, newVal))
            {
                Value = newVal;
                OnChangedValue?.Invoke(Value);
                OnChanged?.Invoke();
            }
        }
    }
}