using System.Collections.Generic;
using Data.Database;
using UnityEngine;

namespace Core.Events
{
    public readonly struct SfxPlayRequest : IEvent
    {
        public IEnumerable<AudioClip> Clips { get; }

        public SfxPlayRequest(IEnumerable<AudioClip> clips)
        {
            Clips = clips;
        }

        public SfxPlayRequest(AudioClip clip) : this(new[] { clip })
        {
            
        }

        public SfxPlayRequest(string audioId) : this(ResourceDatabases.Sounds[audioId])
        {
            
        }
    }

    public readonly struct MusicPlayRequest : IEvent
    {
        public AudioClip Clip { get; }
        public MusicPlayRequest(AudioClip clip) => Clip = clip;
    }
}