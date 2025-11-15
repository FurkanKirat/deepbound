using Data.Database;
using Data.Serializable.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.Models.References
{
    [JsonConverter(typeof(SpriteRefConverter))]
    public class SpriteRef : BaseRef<Sprite>
    {
        public SpriteRef(string key) : base(key, ResourceDatabases.Sprites)
        {
        }
        
        private sealed class SpriteRefConverter : BaseRefConverter<SpriteRef, Sprite>
        {
            protected override SpriteRef CreateRef(string key) => new (key);
        }
    }
}