using System;

namespace Core.Events
{
    public struct GlobalSavedEvent : IEvent
    {
        public DateTime Timestamp;

        private GlobalSavedEvent(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
        
        public static GlobalSavedEvent Now => new(DateTime.UtcNow);
    }

    public struct DimensionSavedEvent : IEvent
    {
        public string DimensionId;

        public DimensionSavedEvent(string dimensionId)
        {
            DimensionId = dimensionId;
        }
    }

    public readonly struct SaveGlobalRequest : IEvent
    {
        
    }

    public readonly struct SaveDimensionRequest : IEvent
    {
        
    }

    public readonly struct SaveWorldRequest : IEvent
    {
        public bool SaveScreenshot { get; }

        public SaveWorldRequest(bool saveScreenshot)
        {
            SaveScreenshot = saveScreenshot;
        }
    }
}