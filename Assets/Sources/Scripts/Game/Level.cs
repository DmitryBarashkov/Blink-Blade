using System;
using System.Collections.Generic;
using UniRx;
using YG;
using Zenject;

public class Level : IDisposable
{
    public event Action LevelFinished;
    public event Action LevelStarted;
    
    private LevelState _levelState;
    private InputService _input;
    
    public int EnemiesCount => _enemiesCount;
    public int LevelNumber => _levelNumber;

    private int _enemiesCount;
    private int _levelNumber;
    private bool _shouldReload;
    
    private AudioService _audioService;
    private BetweenLevelScreen _menu;
    
    private ActiveLevelBridge _levelBridge;
    private PlayerSpawner _playerSpawner;
    private EnemySpawner _enemySpawner;
    private CameraBoundsInstaller _cameraController;
    private EnemyPanel _enemyPanel;

    private IReadOnlyList<ArrowTrap> _arrowTraps;
    
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly SerialDisposable _enemiesSubscription = new SerialDisposable();

    [Inject]
    public void Construct(AudioService audioService, ActiveLevelBridge levelBridge, EnemyPanel enemyPanel,
                          CameraBoundsInstaller cameraController, EnemySpawner enemySpawner, PlayerSpawner playerSpawner,
                          LevelState levelState, InputService input, [Inject(Optional = true)] BetweenLevelScreen menu)
    {
        _levelBridge = levelBridge;
        _enemySpawner = enemySpawner;
        _playerSpawner = playerSpawner;
        _cameraController = cameraController;        

        _levelNumber = YG2.saves.level;
        _audioService = audioService;        
        _levelState = levelState;
        _input = input;
        _menu = menu;

        _enemyPanel = enemyPanel;

        Initialize();
    }

    public void ShowMenu()
    {
        if (_menu == null)
            return;

        _input.Deactivate();        
        _menu.Activate();
    }

    public void StartPlay()
    {
        _input.Activate();

        if (_shouldReload)
        {
            ActivateTraps();
            ActivateEnemies();
            LevelStarted?.Invoke();
        }
        else
            ActivatePlayer();
    }

    public void Win()
    {
        _input.Deactivate();
        _levelState.FinishLevel(true);
        _audioService.PlaySound(SoundType.Win);

        LevelFinished?.Invoke();
    }

    public void Lose(bool isOutOfEnergy = false)
    {
        _input.Deactivate();
        _levelState.FinishLevel(false, isOutOfEnergy);
        _audioService.PlaySound(SoundType.Lose);

        LevelFinished?.Invoke();
    }

    public void Restart(bool isNeedInputActivate = true)
    {
        _enemySpawner.Reset();
        _playerSpawner.Reset();
        _enemyPanel.Reset();

        _levelState.Restart(_enemiesCount);

        if (isNeedInputActivate)
            _input.Activate();
    }

    public void SetReload(bool value)
    {
        _shouldReload = value;
    }

    public void Dispose() => _disposables.Dispose();

    private void Initialize()
    {
        _audioService.PlayAmbient(SoundType.AmbientSounds);
        _audioService.PlayMusic(SoundType.BackgroundMusic);

        _levelBridge.CurrentLevel
            .Where(level => level != null)
            .Subscribe(levelData =>
            {
                InitializeLevelData(levelData);
                InitializeLevelState(levelData);
            })
            .AddTo(_disposables);
    }

    private void InitializeLevelState(ILevelData levelData)
    {
        _shouldReload = true;

        _enemiesCount = levelData.GetEnemySpawnPoints().Count;
        _levelState.Restart(_enemiesCount);        

        _enemiesSubscription.Disposable = _levelState.CurrentEnemiesCount
            .Subscribe(enemiesCount =>
            {
                _enemyPanel.UpdateIcons(enemiesCount);

                if (enemiesCount == 0)
                {
                    Win();
                }
            });            
    }

    private void InitializeLevelData(ILevelData levelData)
    {
        _enemySpawner.Initialize(levelData);
        _playerSpawner.Initialize(levelData);
        _cameraController.Initialize(levelData);
        _enemyPanel.Initialize(levelData);
        _arrowTraps = levelData.GetArrowTraps();
        _levelNumber = YG2.saves.level;

        YG2.GetLeaderboard("Score");

        ShowMenu();
    }

    private void ActivateTraps()
    {
        if (_arrowTraps.Count > 0)
            foreach (var trap in _arrowTraps)
                trap.Activate();
    }

    private void ActivateEnemies()
    {
        _enemySpawner.ActivateEnemies();
    }

    private void ActivatePlayer()
    {
        _playerSpawner.ActivateAfterEnergyAdded();
    }
}
