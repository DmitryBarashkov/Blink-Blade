using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Zenject;

public enum SoundType
{
    BackgroundMusic,

    ForestAmbientSounds,
    DarkForestAmbientSounds,
    CaveAmbientSounds,

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
    WeaponStoneHit,

    FallingOnGround,
    CastScream,
}

public interface IAudioService
{
    void Activate();
    void Deactivate();
    void PlaySound(SoundType type);
    void PlayMusic();
    void StopMusic();
    void SetAmbientSound(SoundType type);
    void PlayAmbient();
    void StopAmbient();

    bool GetSoundOn();
    void SetSound(bool value);
}

[Serializable]
public struct SoundData
{
    public SoundType Type;
    public AudioClip Clip;
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

    private SoundType _ambientSoundType = SoundType.ForestAmbientSounds;

    [Inject]
    public void Construct()
    {
        _isSoundOn = YG2.saves.IsSoundOn;
    }

    public void Activate()
    {
        _sfxSource.enabled = true;
        _ambientSource.enabled = true;
        _musicSource.enabled = true;

        PlayAmbient();
        PlayMusic();
    }

    public void Deactivate()
    {
        _sfxSource.enabled = false;
        _ambientSource.enabled = false;
        _musicSource.enabled = false;
    }

    public bool GetSoundOn()
    {
        return _isSoundOn;
    }

    public void SetSound(bool value)
    {
        _isSoundOn = value;

        YG2.saves.IsSoundOn = value;
        YG2.SaveProgress();

        if (_isSoundOn)
        {
            _sfxSource.enabled = true;
            _ambientSource.enabled = true;
            _musicSource.enabled = true;
            PlayAmbient();
            PlayMusic();
        }
        else
        {
            _musicSource.enabled = false;
            _sfxSource.enabled = false;
            _ambientSource.enabled = false;
            StopAmbient();
            StopMusic();
        }
    }

    public void PlaySound(SoundType type)
    {
        if (_isSoundOn)
        {
            AudioClip clip = GetClip(type);

            _sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic()
    {
        if (_isSoundOn)
        {
            AudioClip clip = GetClip(SoundType.BackgroundMusic);

            _musicSource.clip = clip;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }

    public void SetAmbientSound(SoundType type)
    {
        _ambientSoundType = type;
    }

    public void PlayAmbient()
    {
        if (_isSoundOn)
        {
            AudioClip clip = GetClip(_ambientSoundType);

            _ambientSource.clip = clip;
            _ambientSource.loop = true;
            _ambientSource.Play();
        }
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
            Debug.Log($"AudioClip для звука {type} не назначен в инспекторе префаба!");

        return sound.Clip;
    }
}
