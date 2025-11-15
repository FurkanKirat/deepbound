namespace Core.Events
{
    public struct MinimapChunkDiscoveredEvent : IEvent
    {
        public int ChunkX { get; }
        public int ChunkY { get; }

        public MinimapChunkDiscoveredEvent(int chunkX, int chunkY)
        {
            ChunkX = chunkX;
            ChunkY = chunkY;
        }
    }
}