using System;
using UniRx;
using YG;
using Zenject;

public class Level: IInitializable
{
    public event Action LevelFinished;
    
    private LevelState _levelState;
    private InputService _input;
    
    public int EnemiesCount => _enemiesCount;
    public int LevelNumber => _levelNumber;

    private int _enemiesCount;
    private int _levelNumber;
    private bool _shouldReload = true;
    private LevelRestartService.Factory _restartFactory;
    private AudioService _audioService;
    private EnemySpawner _enemySpawner;
    private BetweenLevelScreen _menu;

    public Level(EnemySpawner enemySpawner, LevelRestartService.Factory restartFactory, AudioService audioService, 
                 LevelState levelState, InputService input, [Inject(Optional = true)] BetweenLevelScreen menu)
    {
        _restartFactory = restartFactory;
        _levelNumber = YG2.saves.level;
        _audioService = audioService;
        _enemySpawner = enemySpawner;
        _levelState = levelState;
        _input = input;
        _menu = menu;
    }

    public void Initialize()
    {
        _audioService.PlayAmbient(SoundType.AmbientSounds);
        _audioService.PlayMusic(SoundType.BackgroundMusic);
        
        _levelState.CurrentEnemiesCount.Value = _enemiesCount = _enemySpawner.GetEnemiesCount();
        _levelState.CurrentEnemiesCount
            .Subscribe(enemiesCount =>
            {
                if (enemiesCount == 0)
                    Win();
            });
    }

    public void ShowMenu()
    {
        if (_menu == null)
            return;
        
        _input.Deactivate();
        _shouldReload = false;
        _menu.Activate();
    }

    public void StartPlay()
    {
        _input.Activate();

        if (_shouldReload)
            _enemySpawner.ActivateAllEnemies();
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

    public void Restart()
    {
        var restartService = _restartFactory.Create();
        
        restartService.ExecuteRestart();
        _levelState.Restart(_enemiesCount);

        _input.Activate();
    }

    public void SetReload(bool value)
    {
        _shouldReload = value;
    }
}
