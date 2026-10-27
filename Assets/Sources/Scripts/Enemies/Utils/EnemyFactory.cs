using BlinkBlade.Game;
using UnityEngine;
using Zenject;

namespace BlinkBlade.Enemies
{
    public class EnemyFactory
    {
        private readonly DiContainer Container;

        public EnemyFactory(DiContainer container)
        {
            Container = container;
        }

        public Enemy Create(EnemySpawnPoint spawnPoint, Transform enemyContainer, ILevelData levelData)
        {
            Enemy enemy = spawnPoint.Enemy;
            Transform enemyTransform = spawnPoint.transform;

            if (enemy != null)
            {
                return Container.InstantiatePrefabForComponent<Enemy>(
                    enemy,
                    enemyTransform.position,
                    enemyTransform.rotation,
                    enemyContainer,
                    new object[] { levelData });
            }

            Debug.Log($"Не удалось создать врага {enemy.name}");

            return null;
        }
    }
}