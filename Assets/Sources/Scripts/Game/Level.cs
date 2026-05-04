using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Level
{
    [Inject] private LevelStats _levelStats;
    [Inject]private List<EnemySpawnPoint> _spawnPoints;

    private InputService _input;

    private int _currentEnemiesCount;

    [Inject]
    private void Construct(InputService input)
    {
        _input = input;        
        _currentEnemiesCount = _spawnPoints.Count;
        _levelStats.currentEnemiesCount.Value = _currentEnemiesCount;
    }

    public void StartPlay()
    {
        _input.Activate();
    }

    public void DecreaseEnemiesCount()
    {
        _currentEnemiesCount--;
        _levelStats.currentEnemiesCount.Value = _currentEnemiesCount;

        if (_currentEnemiesCount == 0)
            Win();
    }

    private void Win()
    {
        Debug.Log("Вы победили");
    }
}
