using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private List<EnemySpawnPoint> _spawnPoints;
    
    public override void InstallBindings()
    {
        BindEnemies();
        BindLevel();
    }

    private void BindLevel()
    {
        Container.BindInterfacesAndSelfTo<Level>().AsSingle().NonLazy();
    }

    private void BindEnemies()
    {
        _spawnPoints.ForEach((EnemySpawnPoint spawnPoint) =>
        {
            Container.BindInterfacesAndSelfTo<Enemy>()
                .FromComponentInNewPrefab(spawnPoint.EnemyPrefab)
                .AsTransient()
                .OnInstantiated<Enemy>((ctx, enemy) =>
                {
                    enemy.transform.position = spawnPoint.transform.position;
                    enemy.transform.rotation = spawnPoint.transform.rotation;
                })
                .NonLazy();
        });
    }
}
