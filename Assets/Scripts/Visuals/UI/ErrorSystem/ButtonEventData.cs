using System;

namespace Visuals.UI.ErrorSystem
{
    public class ButtonEventData
    {
        public string Text { get; }
        public Action OnClick { get; }

        public ButtonEventData(string text, Action onClick)
        {
            Text = text;
            OnClick = onClick;
        }
    }
}