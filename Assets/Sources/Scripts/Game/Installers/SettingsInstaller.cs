using UnityEngine;
using Zenject;

public class SettingsInstaller : MonoInstaller
{
    [SerializeField] private RectTransform _settingsContainer;
    [SerializeField] private SoundButton _soundButton;
    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private LanguageButton _languageButton;

    public override void InstallBindings()
    {
        BindOptionsUI();
    }

    private void BindOptionsUI()
    {
        Container.BindInterfacesAndSelfTo<SoundButton>()
            .FromComponentInNewPrefab(_soundButton)
            .UnderTransform(_settingsContainer)
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<MusicButton>()
            .FromComponentInNewPrefab(_musicButton)
            .UnderTransform(_settingsContainer)
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<LanguageButton>()
            .FromComponentInNewPrefab(_languageButton)
            .UnderTransform(_settingsContainer)
            .AsSingle()
            .NonLazy();
    }
}
