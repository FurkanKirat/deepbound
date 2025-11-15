namespace Core.Events
{
    public readonly struct LanguageChangedEvent : IEvent
    {
        public readonly string Key;

        public LanguageChangedEvent(string key)
        {
            Key = key;
        }
    }
}