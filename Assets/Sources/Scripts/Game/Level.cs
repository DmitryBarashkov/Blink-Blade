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
    
    private int _enemiesCount;
    private int _levelNumber;
        
    private AudioService _audioService;
    private BetweenLevelScreen _menu;
    private LevelScreen _levelUI;
    
    private ActiveLevelBridge _levelBridge;
    private PlayerSpawner _playerSpawner;
    private EnemySpawner _enemySpawner;
    private CameraBoundsInstaller _cameraController;
    private EnemyPanel _enemyPanel;

    private IReadOnlyList<ArrowTrap> _arrowTraps;
    private ILevelData _levelData;
    
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly SerialDisposable _enemiesSubscription = new SerialDisposable();
    
    public int EnemiesCount => _enemiesCount;
    public int LevelNumber => _levelNumber;

    [Inject]
    public void Construct(AudioService audioService, ActiveLevelBridge levelBridge, EnemyPanel enemyPanel,
                          CameraBoundsInstaller cameraController, EnemySpawner enemySpawner, PlayerSpawner playerSpawner,
                          LevelState levelState, InputService input,
                          [Inject(Optional = true)] LevelScreen levelUI,
                          [Inject(Optional = true)] BetweenLevelScreen menu)
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
        _levelUI = levelUI;

        Initialize();
    }

    public void ShowMenu()
    {
        if (_menu == null)
            return;

        _input.Deactivate();
        _levelUI?.SetActive(false);
        _menu.Activate();
    }

    public void StartPlay(bool isContinue = false)
    {
        _input.Activate();
        _levelUI?.SetActive(true);

        ActivateTraps();
        ActivateEnemies(isContinue);
        ActivatePlayer();

        LevelStarted?.Invoke();
    }

    public void Lose(bool isOutOfEnergy = false)
    {
        _input.Deactivate();
        _levelUI?.SetActive(false);
        _levelState.FinishLevel(false, isOutOfEnergy);
        _audioService.PlaySound(SoundType.Lose);

        DeactivateEnemies();
        DeactivateTraps();

        LevelFinished?.Invoke();
    }

    public void Restart(bool isNeedInputActivate = true)
    {
        _enemySpawner.Reset();
        _playerSpawner.Initialize(_levelData.GetPlayerSpawnPoint());
        _enemyPanel.Reset();
        
        ActivateTraps();

        _levelState.Restart(_enemiesCount);

        if (isNeedInputActivate)
            _input.Activate();

        _levelUI?.SetActive(true);
    }

    public void Dispose() => _disposables.Dispose();

    private void Initialize()
    {
        _audioService.PlayMusic();

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
        _enemiesCount = levelData.IsBossLevel() ? levelData.GetBossHealth() : levelData.GetEnemySpawnPoints().Count;
        _levelState.Restart(_enemiesCount);        

        _enemiesSubscription.Disposable = _levelState.CurrentEnemiesCount
            .Subscribe(enemiesCount =>
            {
                _enemyPanel.UpdateIcons(enemiesCount);

                if (enemiesCount == 0)
                    Win();                
            });            
    }

    private void InitializeLevelData(ILevelData levelData)
    {

        _audioService.SetAmbientSound(levelData.GetAmbientSoundType());
        _audioService.Activate();        

        _levelData = levelData;
        
        _enemySpawner.Initialize(levelData);
        _playerSpawner.Initialize(levelData.GetPlayerSpawnPoint());
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

    private void DeactivateTraps()
    {
        if (_arrowTraps.Count > 0)
            foreach (var trap in _arrowTraps)
                trap.Deactivate();
    }

    private void ActivateEnemies(bool isContinue)
    {
        _enemySpawner.ActivateEnemies(isContinue);
    }

    private void DeactivateEnemies()
    {
        _enemySpawner.DeactivateEnemies();
    }

    private void ActivatePlayer()
    {
        _playerSpawner.ActivatePlayer();
    }

    private void Win()
    {
        _input.Deactivate();
        _levelUI?.SetActive(false);
        _levelState.FinishLevel(true);
        _audioService.PlaySound(SoundType.Win);

        DeactivateEnemies();
        DeactivateTraps();

        LevelFinished?.Invoke();
    }
}
