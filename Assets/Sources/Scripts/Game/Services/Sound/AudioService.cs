using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Zenject;

public enum SoundType
{
    BackgroundMusic,
    AmbientSounds,
    
    ButtonClick,
    ExpandPanel,
    Win,
    Lose,

    ThrowWeapon,
    SwordAttack,
    ArcherStartAim,
    Teleport,
    Hurt,
    WeaponGrassHit,
    BowShot,
    WeaponMetalHit,
    WeaponWoodHit,

    FallingOnGround,
}

[Serializable]
public struct SoundData
{
    public SoundType Type;
    public AudioClip Clip;
}

public interface IAudioService
{
    void PlaySound(SoundType type);
    void PlayMusic(SoundType type);
    void PlayAmbient(SoundType type);
    void StopMusic();
    void StopAmbient();

    bool GetSoundOn();
    bool GetMusicOn();
    void SetSound(bool value);
    void SetMusic(bool value);

}

public class AudioService : MonoBehaviour, IAudioService
{
    [Header("Sources")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _ambientSource;

    [Header("Audio Clips")]
    [SerializeField] private List<SoundData> _sounds;

    private bool _isSoundOn;
    private bool _isMusicOn;

    [Inject]
    public void Construct()
    {
        _isMusicOn = YG2.saves.isMusicOn;
        _isSoundOn = YG2.saves.isSoundOn;
    }
    
    public bool GetSoundOn()
    {
        return _isSoundOn;
    }

    public bool GetMusicOn()
    {
        return _isMusicOn;
    }

    public void SetSound(bool value)
    {
        _isSoundOn = YG2.saves.isSoundOn = value;

        if (_isSoundOn)
        {
            _sfxSource.enabled = true;
            PlayAmbient(SoundType.AmbientSounds);
        }            
        else
        {
            _sfxSource.enabled = false;
            StopAmbient();
        }
    }

    public void SetMusic(bool value)
    {
        _isMusicOn = YG2.saves.isMusicOn = value;

        if (_isMusicOn)
            PlayMusic(SoundType.BackgroundMusic);
        else
            StopMusic();
    }

    public void PlaySound(SoundType type)
    {
        AudioClip clip = GetClip(type);
        
        _sfxSource.PlayOneShot(clip);        
    }

    public void PlayMusic(SoundType type)
    {
        AudioClip clip = GetClip(type);
        
        _musicSource.clip = clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlayAmbient(SoundType type)
    {
        AudioClip clip = GetClip(type);

        _ambientSource.clip = clip;
        _ambientSource.loop = true;
        _ambientSource.Play();
    }

    public void StopAmbient()
    {
        _ambientSource.Stop();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    private AudioClip GetClip(SoundType type)
    {
        var sound = _sounds.Find(s => s.Type == type);
        
        if (sound.Clip == null)
        {
            Debug.Log($"AudioClip для звука {type} не назначен в инспекторе префаба!");
        }

        return sound.Clip;
    }
}
