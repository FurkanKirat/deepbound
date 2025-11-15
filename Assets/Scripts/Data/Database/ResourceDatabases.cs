using Generated.Paths;
using UnityEngine;

namespace Data.Database
{
    public static class ResourceDatabases
    {
        public static ResourceOnDemandDatabase<Sprite> Sprites { get; private set; }
        public static ResourceDatabase<AudioClip> Sounds { get; private set; }
        public static ResourceDatabase<GameObject> Prefabs { get; private set; }

        public static void LoadAll()
        {
            Sprites = new ResourceOnDemandDatabase<Sprite>();
            
            Prefabs = new ResourceDatabase<GameObject>();
            Prefabs.LoadFromFolders(ResourcesDataPaths.PrefabsRoot);
            
            Sounds = new ResourceDatabase<AudioClip>();
            Sounds.LoadFromFolder(ResourcesDataPaths.AudioRoot);
        }
    }
}