using System;
using System.Collections.Generic;
using BlinkBlade.UI;
using UniRx;
using UnityEngine;
using YG;
using Zenject;

namespace BlinkBlade.Game
{
    public class UIService : IInitializable, IDisposable
    {
        private readonly LevelState LevelState;
        private readonly LevelLoadService LoadService;
        private readonly DiContainer Container;

        private readonly Dictionary<Component, GameObject> CachedWindows = new ();
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        private UIScreen _winScreenPrefab;
        private UIScreen _loseScreenPrefab;
        private UIScreen _shopScreenPrefab;
        private UIScreen _finishScreenPrefab;

        private Transform _endGameContainer;
        private Transform _shopContainer;

        private float _showDelay = 0.5f;

        public UIService(
            LevelState levelState,
            DiContainer container,
            LevelLoadService loadService,
            UIScreen winScreenPrefab,
            UIScreen loseScreenPrefab,
            UIScreen finishScreenPrefab,
            [Inject(Optional = true)] UIScreen shopScreenPrefab,
            Transform endGameContainer,
            Transform shopContainer)
        {
            LevelState = levelState;
            LoadService = loadService;
            Container = container;
            _winScreenPrefab = winScreenPrefab;
            _loseScreenPrefab = loseScreenPrefab;
            _finishScreenPrefab = finishScreenPrefab;
            _shopScreenPrefab = shopScreenPrefab;
            _endGameContainer = endGameContainer;
            _shopContainer = shopContainer;
        }

        public void Initialize()
        {
            LevelState.IsWin
                .Delay(TimeSpan.FromSeconds(_showDelay), Scheduler.MainThreadIgnoreTimeScale)
                .ObserveOnMainThread()
                .Subscribe(isWin =>
                {
                    if (isWin.HasValue)
                        OnLevelFinished(isWin ?? false);
                }).AddTo(Disposables);
        }

        public void ShowShop()
        {
            GameObject shop = GetOrCreateWindow(_shopScreenPrefab, _shopContainer);
            ShopScreen screen = shop.GetComponent<ShopScreen>();

            screen.Setup();
        }

        public void Dispose() => Disposables.Dispose();

        private void OnLevelFinished(bool isWin)
        {
            UIScreen targetPrefab = GetEndGameScreen(isWin);
            GameObject window = GetOrCreateWindow(targetPrefab, _endGameContainer);
            UIScreen endGameScreen = window.GetComponent<EndGameScreen>();

            endGameScreen.Setup();
        }

        private GameObject GetOrCreateWindow(UIScreen prefab, Transform container)
        {
            if (CachedWindows.TryGetValue(prefab, out GameObject activeWindow))
                return activeWindow;

            GameObject spawnedInstance = Container.InstantiatePrefab(prefab, container);

            CachedWindows[prefab] = spawnedInstance;

            return spawnedInstance;
        }

        private UIScreen GetEndGameScreen(bool isWin)
        {
            if (isWin == false)
                return _loseScreenPrefab;

            if (YG2.saves.Level == LoadService.LastLevelNumber && YG2.saves.IsFinishedGame == false)
            {
                YG2.saves.IsFinishedGame = true;
                YG2.SaveProgress();

                return _finishScreenPrefab;
            }

            return _winScreenPrefab;
        }
    }
}