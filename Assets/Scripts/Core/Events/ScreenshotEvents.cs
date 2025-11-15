namespace Core.Events
{
    public struct ScreenshotRequest : IEvent
    {
        public string Path { get; }

        public ScreenshotRequest(string path)
        {
            Path = path;
        }
    }
}