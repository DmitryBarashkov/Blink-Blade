using UnityEngine;
using YG;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [Header("Buttons")]
    [SerializeField] private SoundButton _soundButton;
    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private ShopButton _shopButtonPrefab;
    [SerializeField] private LanguageButton _languageButton;
    [SerializeField] private NoAdsButton _noAdsButtonPrefab;


    [Header("Containers")]
    [SerializeField] private RectTransform _settingsContainer;    
    [SerializeField] private RectTransform _betweenLevelContainer;
    [SerializeField] private RectTransform _shopButtonContainer;

    public override void InstallBindings()
    {
        BindOptionsUI();
        BindButtons();
    }

    private void BindButtons()
    {
        if (YG2.saves.isAdsDisabled == false)
        {
            Container.BindInterfacesAndSelfTo<NoAdsButton>()
                .FromComponentInNewPrefab(_noAdsButtonPrefab)
                .UnderTransform(_betweenLevelContainer)
                .AsSingle()
                .NonLazy();
        }

        Container.Bind<ShopButton>()
            .FromComponentInNewPrefab(_shopButtonPrefab)
            .UnderTransform(_shopButtonContainer)
            .AsSingle()
            .NonLazy();
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
