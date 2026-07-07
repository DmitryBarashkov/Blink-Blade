using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using YG;
using Zenject;

public class UIService : IInitializable, IDisposable
{
    private readonly LevelState _levelState;
    private readonly LevelLoadService _loadService;
    private readonly DiContainer _container;

    private readonly UIScreen _winScreenPrefab;
    private readonly UIScreen _loseScreenPrefab;
    private readonly UIScreen _shopScreenPrefab;
    private readonly UIScreen _finishScreenPrefab;

    private Transform _endGameContainer;
    private Transform _shopContainer;

    private readonly Dictionary<Component, GameObject> _cachedWindows = new();
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private float _showDelay = 0.5f;

    public UIService(LevelState levelState, DiContainer container, LevelLoadService loadService,
                     UIScreen winScreenPrefab, UIScreen loseScreenPrefab, UIScreen finishScreenPrefab, 
                     [Inject(Optional = true)] UIScreen shopScreenPrefab,
                     Transform endGameContainer, Transform shopContainer)
    {
        _levelState = levelState;
        _loadService = loadService;
        _container = container;
        _winScreenPrefab = winScreenPrefab;
        _loseScreenPrefab = loseScreenPrefab;
        _finishScreenPrefab = finishScreenPrefab;
        _shopScreenPrefab = shopScreenPrefab;
        _endGameContainer = endGameContainer;
        _shopContainer = shopContainer;
    }

    public void Initialize()
    {
        _levelState.IsWin
            .Delay(TimeSpan.FromSeconds(_showDelay), Scheduler.MainThreadIgnoreTimeScale)
            .ObserveOnMainThread()
            .Subscribe(isWin =>
            {
                if (isWin.HasValue)
                {
                    OnLevelFinished(isWin ?? false);
                }
            }).AddTo(_disposables);
    }

    public void ShowShop()
    {
        GameObject shop = GetOrCreateWindow(_shopScreenPrefab, _shopContainer);
        ShopScreen screen = shop.GetComponent<ShopScreen>();

        screen.Setup();
    }

    public void Dispose() => _disposables.Dispose();

    private void OnLevelFinished(bool isWin)
    {
        UIScreen targetPrefab = GetEndGameScreen(isWin);
        GameObject window = GetOrCreateWindow(targetPrefab, _endGameContainer);
        UIScreen endGameScreen = window.GetComponent<EndGameScreen>();

        endGameScreen.Setup();
    }

    private GameObject GetOrCreateWindow(UIScreen prefab, Transform container)
    {
        if (_cachedWindows.TryGetValue(prefab, out GameObject activeWindow))
        {
            return activeWindow;
        }

        GameObject spawnedInstance = _container.InstantiatePrefab(prefab, container);

        _cachedWindows[prefab] = spawnedInstance;
        
        return spawnedInstance;
    }

    private UIScreen GetEndGameScreen(bool isWin)
    {
        UIScreen targetPrefab = isWin ? _winScreenPrefab : _loseScreenPrefab;

        if (isWin == false)
            return _loseScreenPrefab;
        else
        {
            if (YG2.saves.level == _loadService.LastLevelNumber && YG2.saves.isFinishedGame == false && YG2.reviewCanShow)
            {
                YG2.saves.isFinishedGame = true;
                YG2.SaveProgress();
                
                return _finishScreenPrefab;
            }
            else
                return _winScreenPrefab;
        }        
    }
}
