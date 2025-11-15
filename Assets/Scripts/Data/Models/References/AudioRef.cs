using Data.Database;
using Data.Serializable.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.Models.References
{
    [JsonConverter(typeof(AudioRefConverter))]
    public class AudioRef : BaseRef<AudioClip>
    {
        public AudioRef(string key) : base(key, ResourceDatabases.Sounds)
        {
        }
        
        private sealed class AudioRefConverter : BaseRefConverter<AudioRef, AudioClip>
        {
            protected override AudioRef CreateRef(string key) => new (key);
        }
    }
}