namespace Core.Events
{
    public class StatProvidersChangedEvent : IEvent
    {
        public object Owner { get; }
        public StatProvidersChangedEvent(object owner)
        {
            Owner = owner;
        }
    }

}