using UniRx;
using YG;
using Zenject;

public class Level: IInitializable
{
    private LevelState _levelState;
    private InputService _input;
    
    public int EnemiesCount => _enemiesCount;
    public int LevelNumber => _levelNumber;

    private int _enemiesCount;
    private int _levelNumber;
    private LevelRestartService.Factory _restartFactory;
    private AudioService _audioService;
    private EnemySpawner _enemySpawner;

    public Level(EnemySpawner enemySpawner, LevelRestartService.Factory restartFactory, AudioService audioService, LevelState levelState, InputService input)
    {
        _restartFactory = restartFactory;
        _levelNumber = YG2.saves.level;
        _audioService = audioService;
        _enemySpawner = enemySpawner;
        _levelState = levelState;
        _input = input;
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

    public void StartPlay(bool isReloadLevel = true)
    {
        _input.Activate();

        if (isReloadLevel)
            _enemySpawner.ActivateAllEnemies();
    }

    public void Win()
    {
        _input.Deactivate();
        _levelState.FinishLevel(true);
        _audioService.PlaySound(SoundType.Win);
    }

    public void Lose(bool isOutOfEnergy = false)
    {
        _input.Deactivate();
        _levelState.FinishLevel(false, isOutOfEnergy);
        _audioService.PlaySound(SoundType.Lose);
    }

    public void Restart()
    {
        var restartService = _restartFactory.Create();
        
        restartService.ExecuteRestart();
        _levelState.Restart(_enemiesCount);

        _input.Activate();
    }
}
