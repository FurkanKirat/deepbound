using System.IO;
using Core;
using Core.Events;
using Systems.WorldSystem;
using Utils;

namespace Systems
{
    public static class WorldDeleter
    {
        public static void DeleteWorld(WorldMetaData metaData, bool removeCompletely)
        {
            var worldPath = WorldPathUtils.GetWorldFolder(metaData);
            if (removeCompletely)
            {
                Directory.Delete(worldPath, true);
            }
            else
            {
                var deletedDirectory = WorldPathUtils.GetDeletedWorldFolder(metaData);
                
                if (Directory.Exists(deletedDirectory))
                    Directory.Delete(deletedDirectory, true);
                
                Directory.Move(worldPath, deletedDirectory);
            }
            GameEventBus.Publish(new WorldDeletedEvent(metaData));
        }
    }
}