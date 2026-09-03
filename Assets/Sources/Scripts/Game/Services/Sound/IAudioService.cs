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
