using UniRx;
using YG;
using Zenject;

public class Level
{
    [Inject] private LevelState _levelState;
    [Inject] private InputService _input;    
    
    public int EnemiesCount => _enemiesCount;
    public int LevelNumber => _levelNumber;

    private int _enemiesCount;
    private int _levelNumber;
    private LevelRestartService.Factory _restartFactory;

    [Inject]
    private void Construct(int enemiesCount, LevelRestartService.Factory restartFactory)
    {
        _levelState.CurrentEnemiesCount.Value = _enemiesCount = enemiesCount;
        _restartFactory = restartFactory;
        _levelNumber = YG2.saves.level;

        _levelState.CurrentEnemiesCount
            .Subscribe(enemiesCount =>
            {
                if (enemiesCount == 0)
                    Win();
            });
    }

    public void StartPlay()
    {
        _input.Activate();
    }

    public void Win()
    {
        _input.Deactivate();
        _levelState.FinishLevel(true);
    }

    public void Lose(bool isOutOfEnergy = false)
    {
        _input.Deactivate();
        _levelState.FinishLevel(false, isOutOfEnergy);
    }

    public void Restart()
    {
        var restartService = _restartFactory.Create();
        
        restartService.ExecuteRestart();
        _levelState.Restart(_enemiesCount);

        _input.Activate();
    }
}
