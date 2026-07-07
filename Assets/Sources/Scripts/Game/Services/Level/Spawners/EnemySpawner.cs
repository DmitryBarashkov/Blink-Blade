using System.Collections.Generic;
using UnityEngine;
using Zenject;

using static UnityEngine.Object;

public class EnemySpawner
{
    private readonly EnemyFactory _enemyFactory;
    private readonly List<Enemy> _enemies = new List<Enemy>();    
    private Transform _enemyContainer;

    [Inject]
    public EnemySpawner(EnemyFactory enemyFactory, Transform enemyContainer)
    {
        _enemyFactory = enemyFactory;
        _enemyContainer = enemyContainer;        
    }

    public void Initialize(ILevelData levelData)
    {
        if (levelData != null)
        {
            IReadOnlyList<EnemySpawnPoint> spawnPoints = levelData.GetEnemySpawnPoints();
                    
            if (_enemies.Count > 0)
            {
                foreach (Enemy enemy in _enemies)
                {
                    Destroy(enemy.gameObject);
                }

                _enemies.Clear();
            }

            if (levelData.IsBossLevel())
            {
                EnemySpawnPoint spawnPoint = Utils.GetRandomElement(spawnPoints);
                Enemy enemy = _enemyFactory.Create(spawnPoint, _enemyContainer, levelData);
                
                _enemies.Add(enemy);
            }
            else
            {
                foreach (var spawnPoint in spawnPoints)
                {
                    if (string.IsNullOrEmpty(spawnPoint.selectedEnemyName))
                    {
                        Debug.LogWarning($"На точке спавна {spawnPoint.name} не задан префаб врага!");
                        continue;
                    }

                    Enemy enemy = _enemyFactory.Create(spawnPoint, _enemyContainer, levelData);

                    _enemies.Add(enemy);
                }
            }
        }
    }

    public void ActivateEnemies()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null)
                enemy.Activate();
        }
    }

    public void Reset()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null)
                enemy.Activate();
        }
    }
}
