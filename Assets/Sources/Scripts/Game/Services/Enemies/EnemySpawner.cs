using System.Collections.Generic;
using Zenject;
using UnityEngine;

public class EnemySpawner : IInitializable
{
    [Inject] private readonly List<EnemySpawnPoint> _spawnPoints;
    
    private readonly Enemy.Factory _enemyFactory;
    private readonly List<Enemy> _activeEnemies = new List<Enemy>();
    private Transform _enemyContainer;
    
    public EnemySpawner(Enemy.Factory enemyFactory, Transform container)
    {
        _enemyFactory = enemyFactory;
        _enemyContainer = container;
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

            enemy.SetInitiatePosition(spawnPoint.transform, _enemyContainer);
            
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
