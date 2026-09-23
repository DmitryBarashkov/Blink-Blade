using System.Collections.Generic;

using BlinkBlade.Common;
using BlinkBlade.Enemies;

using UnityEngine;
using Zenject;

using static UnityEngine.Object;

namespace BlinkBlade.Game
{
    public class EnemySpawner
    {
        private readonly EnemyFactory EnemyFactory;
        private readonly List<Enemy> Enemies = new List<Enemy>();
        private Transform _enemyContainer;

        [Inject]
        public EnemySpawner(EnemyFactory enemyFactory, Transform enemyContainer)
        {
            EnemyFactory = enemyFactory;
            _enemyContainer = enemyContainer;
        }

        public void Initialize(ILevelData levelData)
        {
            if (levelData != null)
            {
                IReadOnlyList<EnemySpawnPoint> spawnPoints = levelData.GetEnemySpawnPoints();

                if (Enemies.Count > 0)
                {
                    foreach (Enemy enemy in Enemies)
                    {
                        Destroy(enemy.gameObject);
                    }

                    Enemies.Clear();
                }

                if (levelData.IsBossLevel())
                {
                    Enemy enemy = EnemyFactory.Create(spawnPoints[0], _enemyContainer, levelData);

                    Enemies.Add(enemy);
                }
                else
                {
                    foreach (var spawnPoint in spawnPoints)
                    {
                        if (spawnPoint.Enemy == null)
                        {
                            Debug.LogWarning($"На точке спавна {spawnPoint.name} не задан префаб врага!");
                            continue;
                        }

                        Enemy enemy = EnemyFactory.Create(spawnPoint, _enemyContainer, levelData);

                        Enemies.Add(enemy);
                    }
                }
            }
        }

        public void ActivateEnemies(bool isContinue)
        {
            foreach (Enemy enemy in Enemies)
            {
                if (enemy != null)
                {
                    if (isContinue)
                    {
                        if (enemy.IsDead == false)
                        {
                            enemy.ContinueWork();
                        }
                    }
                    else
                    {
                        enemy.Activate();
                    }
                }
            }
        }

        public void DeactivateEnemies()
        {
            foreach (Enemy enemy in Enemies)
            {
                if (enemy != null)
                    enemy.Deactivate();
            }
        }

        public void Reset()
        {
            foreach (Enemy enemy in Enemies)
            {
                if (enemy != null)
                {
                    enemy.Activate();
                }
            }
        }
    }
}