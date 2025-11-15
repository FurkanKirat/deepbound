using System;
using System.Collections;
using Core;
using Core.Events;
using Utils;
using Visuals.Utils;

namespace Systems.AudioSystem
{
    using UnityEngine;

    public class AudioManager : IDisposable
    {
        private readonly AudioSource _musicSource;
        private readonly ObjectPool<AudioSource> _sfxPool;

        public AudioManager(Transform parent)
        {
            var audioRoot = new GameObject("AudioManager");
            audioRoot.transform.SetParent(parent, false);
            
            _musicSource = audioRoot.AddComponent<AudioSource>();
            _musicSource.loop = true;
            
            _sfxPool = new ObjectPool<AudioSource>(
                factory: () =>
                {
                    var go = new GameObject("SFX_Source");
                    return go.AddComponent<AudioSource>();
                },
                initialSize: 5,
                parent: audioRoot.transform);
            GameEventBus.Subscribe<MusicPlayRequest>(MusicPlayRequest);
            GameEventBus.Subscribe<SfxPlayRequest>(SfxPlayRequest);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<MusicPlayRequest>(MusicPlayRequest);
            GameEventBus.Unsubscribe<SfxPlayRequest>(SfxPlayRequest);
        }
        
        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;
            _musicSource.clip = clip;
            _musicSource.volume = volume;
            _musicSource.Play();
        }

        public void StopMusic() => _musicSource.Stop();

        public void SetMusicVolume(float volume) => _musicSource.volume = volume;
        
        public void PlaySfx(AudioClip clip, float volume = 1f)
        {
            
            if (clip == null)
            {
                GameLogger.Warn($"Sfx is null", nameof(AudioManager));
                return;
            }

            var source = _sfxPool.Get();
            source.clip = clip;
            source.volume = volume;
            source.spatialBlend = 0f;
            source.PlayOneShot(clip);
            GameLogger.Log($"Playing SFX: {clip.name}",nameof(AudioManager));

            GameRoot.Instance.StartCoroutine(ReleaseAfterPlaying(source));
        }

        private void SfxPlayRequest(SfxPlayRequest e)
        {
            foreach (var clip in e.Clips)
            {
                PlaySfx(clip);
            }
        }

        private void MusicPlayRequest(MusicPlayRequest e)
        {
            PlayMusic(e.Clip);
        }

        private IEnumerator ReleaseAfterPlaying(AudioSource source)
        {
            yield return new WaitWhile(() => source.isPlaying);
            _sfxPool.Release(source);
        }
        
    }

}