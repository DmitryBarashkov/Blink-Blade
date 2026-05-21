using System.Collections.Generic;
using Zenject;
using UnityEngine;

public class EnemySpawner : IInitializable
{
    private readonly Enemy.Factory _enemyFactory;
    private readonly List<Enemy> _activeEnemies = new List<Enemy>();

    [Inject] private readonly List<EnemySpawnPoint> _spawnPoints;
    
    public EnemySpawner(Enemy.Factory enemyFactory)
    {
        _enemyFactory = enemyFactory;        
    }

    public void Initialize()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            if (spawnPoint.EnemyPrefab == null)
            {
                Debug.LogWarning($"На точке спавна {spawnPoint.name} не задан префаб врага!");
                continue;
            }

            Enemy enemy = _enemyFactory.Create(spawnPoint.EnemyPrefab);

            enemy.SetInitiatePosition(spawnPoint.transform);

            _activeEnemies.Add(enemy);
        }
    }

    public void ActivateAllEnemies()
    {
        foreach (var enemy in _activeEnemies)
        {
            if (enemy != null)
            {
                enemy.Activate();
            }
        }
    }

    public int GetEnemiesCount()
    {
        return _activeEnemies.Count;
    }
}
