using System.Collections.Generic;
using UniRx;
using UnityEngine.AddressableAssets;
using Zenject;

public class UIService : IInitializable, System.IDisposable
{
    private readonly LevelState _levelState;
    private readonly EndGameWindow.Factory _windowFactory;
    private readonly AssetReference _winReference;
    private readonly AssetReference _loseReference;

    private readonly Dictionary<AssetReference, EndGameWindow> _cachedWindows = new();
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private float _showDelay = 0.5f;

    public UIService(LevelState levelState, EndGameWindow.Factory windowFactory, AssetReference winReference, AssetReference loseReference)
    {
        _levelState = levelState;
        _windowFactory = windowFactory;
        _winReference = winReference;
        _loseReference = loseReference;
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
                    OnLevelFinished(isWin ?? false, _levelState.IsOutOfEnergy.Value);
                }
            })            
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();


    private void OnLevelFinished(bool isWin, bool isOutOfEnergy = false)
    {
        AssetReference targetReference = isWin ? _winReference : _loseReference;

        if (_cachedWindows.TryGetValue(targetReference, out var window))
        {
            window.Setup();
        }
        else
        {
            _windowFactory.Create(targetReference)
                .Subscribe(window =>
                {
                    _cachedWindows[targetReference] = window;
                    window.Setup(isOutOfEnergy);
                })
                .AddTo(_disposables);
        }
    }
}
