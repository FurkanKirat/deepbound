namespace Core.Events
{
    public readonly struct DimensionChangeRequest : IEvent
    {
        public string DimensionId { get; }

        public DimensionChangeRequest(string dimensionId)
        {
            DimensionId = dimensionId;
        }
    }

    public readonly struct DimensionChangedEvent : IEvent
    {
        public string DimensionId { get; }

        public DimensionChangedEvent(string dimensionId)
        {
            DimensionId = dimensionId;
        }
    }
}