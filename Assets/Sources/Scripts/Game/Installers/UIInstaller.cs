using System;
using UnityEngine;
using YG;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private RectTransform _settingsContainer;
    [SerializeField] private SoundButton _soundButton;
    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private LanguageButton _languageButton;
    [SerializeField] private BetweenLevelScreen _betweenLevelScreen;
    [SerializeField] private NoAdsButton _noAdsButtonPrefab;

    public override void InstallBindings()
    {
        BindOptionsUI();
        BindAdsButton();
    }

    private void BindAdsButton()
    {
        if (YG2.saves.isAdsDisabled == false)
        {
            Container.BindInterfacesAndSelfTo<NoAdsButton>()
                .FromComponentInNewPrefab(_noAdsButtonPrefab)
                .UnderTransform(_betweenLevelScreen.transform)
                .AsSingle()
                .NonLazy();
        }
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
