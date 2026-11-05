using BlinkBlade.UI;
using UnityEngine;
using YG;
using Zenject;

namespace BlinkBlade.Game
{
    public class UIInstaller : MonoInstaller
    {
        [Header("Buttons")]
        [SerializeField] private SoundButton _soundButton;
        [SerializeField] private ShopButton _shopButtonPrefab;
        [SerializeField] private LanguageButton _languageButton;
        [SerializeField] private LeaderboardButton _leaderboardButtonPrefab;

        [Header("Containers")]
        [SerializeField] private RectTransform _settingsContainer;
        [SerializeField] private RectTransform _betweenLevelContainer;
        [SerializeField] private RectTransform _shopButtonContainer;
        [SerializeField] private RectTransform _leaderboardButtonContainer;

        public override void InstallBindings()
        {
            BindScreens();
            BindButtons();
        }

        private void BindScreens()
        {
            Container.Bind<BetweenLevelScreen>().FromComponentInHierarchy().AsSingle();

            Container.BindFactory<Transform, GameObject, UIScreen, UIScreen.Factory>()
                .FromMethod((container, parent, prefab) =>
                {
                    GameObject screen = container.InstantiatePrefab(prefab, parent);

                    return screen.GetComponent<UIScreen>();
                });
        }

        private void BindButtons()
        {
            if (YG2.platform == "YandexGames")
            {
                Container.Bind<LeaderboardButton>()
                    .FromComponentInNewPrefab(_leaderboardButtonPrefab)
                    .UnderTransform(_leaderboardButtonContainer)
                    .AsSingle()
                    .NonLazy();
            }

            Container.Bind<ShopButton>()
                .FromComponentInNewPrefab(_shopButtonPrefab)
                .UnderTransform(_shopButtonContainer)
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<SoundButton>()
                .FromComponentInNewPrefab(_soundButton)
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
}