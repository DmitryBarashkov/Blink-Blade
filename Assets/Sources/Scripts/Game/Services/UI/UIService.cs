using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class UIService : IInitializable, System.IDisposable
{
    private readonly LevelState _levelState;
    private readonly UIScreen.Factory _windowFactory;
    private readonly AssetReference _winReference;
    private readonly AssetReference _loseReference;
    private readonly AssetReference _shopReference;
    private Transform _endGameContainer;
    private Transform _shopContainer;
    private readonly Dictionary<AssetReference, UIScreen> _cachedWindows = new();
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private float _showDelay = 0.5f;

    public UIService(LevelState levelState, [Inject(Optional = true)] UIScreen.Factory windowFactory,
                     AssetReference winReference, AssetReference loseReference, AssetReference shopReference,
                     Transform endGameContainer, Transform shopContainer)
    {
        _levelState = levelState;
        _windowFactory = windowFactory;
        _winReference = winReference;
        _loseReference = loseReference;
        _shopReference = shopReference;
        _endGameContainer = endGameContainer;
        _shopContainer = shopContainer;
    }

    public void Initialize()
    {
        _levelState.IsWin
            .Delay(System.TimeSpan.FromSeconds(_showDelay))
            .ObserveOnMainThread()
            .Subscribe(isWin =>
            {
                if (isWin.HasValue)
                {
                    OnLevelFinished(isWin ?? false);
                }
            })            
            .AddTo(_disposables);
    }

    public void ShowShop()
    {
        OpenScreen(_shopReference, _shopContainer, screen =>
        {
            if (screen is ShopScreen shop)
            {
                shop.Setup();
            }
        });
    }

    public void Dispose() => _disposables.Dispose();

    private void OnLevelFinished(bool isWin)
    {
        AssetReference targetReference = isWin ? _winReference : _loseReference;

        OpenScreen(targetReference, _endGameContainer, screen =>
        {
            if (screen is EndGameScreen endGameScreen)
            {
                endGameScreen.Setup();
            }
        });
    }

    private void OpenScreen(AssetReference reference, Transform container, System.Action<UIScreen> onReady)
    {
        if (_cachedWindows.TryGetValue(reference, out var screen))
        {
            onReady?.Invoke(screen);
        }
        else
        {
            _windowFactory.Create(container, reference)
                .ObserveOnMainThread()
                .Subscribe(loadedScreen =>
                {
                    _cachedWindows[reference] = loadedScreen;
                    onReady?.Invoke(loadedScreen);
                })
                .AddTo(_disposables);
        }
    }
}
