using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonPersistent<AudioManager>
{
    [Serializable]
    private class Sound
    {
        public string Tag;
        public AudioClip Clip;
    }
    
    [Serializable]
    private class SoundEffect : Sound
    {
        public SfxType Type;
    }

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private List<SoundEffect> _soundEffects;
    [SerializeField] private List<Sound> _musics;
    [SerializeField] private AudioSource _sfx, _music;

    public bool IsMusicPlaying
    {
        get; set;
    }

    protected override void Awake()
    {
        base.Awake();

        
    }

    public void PlayMusic(string tag)
    {
        Sound sound = SearchSound(tag);

        if (sound == null)
        {
            Debug.LogError("ERROR: Sound is Null");
            return;
        }

        if (IsMusicPlaying) return;

        IsMusicPlaying = true;
        _music.clip = sound.Clip;
        _music.loop = true;
        _music.Play();
    }

    public void StopMusic()
    {
        if (IsMusicPlaying)
        {
            _music.Stop();
            _music.loop = false;
            IsMusicPlaying = false;
        }
    }

    public void PlaySoundEffect(string tag, SfxType type)
    {
        Sound soundEffect = SearchSoundEffect(tag, type);

        if (soundEffect == null)
        {
            Debug.LogError("ERROR: Sound Effect is Null");
            return;
        }

        _sfx.PlayOneShot(soundEffect.Clip);
    }

    private SoundEffect SearchSoundEffect(string tag, SfxType type)
    {
        foreach (var effect in _soundEffects)
        {
            if (effect.Tag == tag && effect.Type == type)
                return effect;
        }

        return null;
    }

    private Sound SearchSound(string tag)
    {
        foreach (var sound in _musics)
        {
            if (sound.Tag == tag)
            {
                return sound;
            }
        }

        return null;
    }

    public void SetMasterVolume(string tag, float volume) => _audioMixer.SetFloat(tag, volume);
}
