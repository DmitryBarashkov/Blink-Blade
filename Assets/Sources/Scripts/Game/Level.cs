using Zenject;
using UniRx;

public class Level
{
    [Inject] private LevelState _levelState;
    [Inject] private InputService _input;
    [Inject] private LevelRestartService _restartService;

    private int _enemiesCount;

    [Inject]
    private void Construct(int enemiesCount)
    {
        _levelState.CurrentEnemiesCount.Value = _enemiesCount = enemiesCount;

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
        _levelState.Restart(_enemiesCount);        
        _restartService.ExecuteRestart();
    }
}
