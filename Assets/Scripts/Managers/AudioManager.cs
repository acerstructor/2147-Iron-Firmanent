using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonPersistent<AudioManager>
{
    private class Sound
    {
        public string Tag;
        public AudioClip Clip;
    }
    
    private class SoundEffect : Sound
    {
        public SfxType Type;
    }

    private AudioMixer _audioMixer;
    private List<SoundEffect> _soundEffects;
    private AudioSource _sfx;

    protected override void Awake()
    {
        base.Awake();

        
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

    public void SetMasterVolume(string tag, float volume) => _audioMixer.SetFloat(tag, volume);
}
