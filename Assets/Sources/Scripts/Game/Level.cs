using System.Text.RegularExpressions;
using UniRx;
using UnityEngine.SceneManagement;
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
        _levelNumber = GetLevelNumber();

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

    private int GetLevelNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Match match = Regex.Match(sceneName, @"\d+");

        if (match.Success)
        {
            return int.Parse(match.Value);
        }

        return -1;
    }
}
