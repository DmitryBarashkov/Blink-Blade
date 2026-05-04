using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private List<EnemySpawnPoint> _spawnPoints;
    [SerializeField] private EnemyPanel _enemyPanelPrefab;
    [SerializeField] private EnemyIcon _enemyIconPrefab;
    [SerializeField] private Canvas _levelUICanvas;

    public override void InstallBindings()
    {
        Debug.Log($"Installing on: {gameObject.name}", gameObject);

        BindEnemies();
        BindLevel();
        BindEnemiesUI();
    }

    private void BindEnemiesUI()
    {
        Container.BindInterfacesAndSelfTo<EnemyPanel>()
            .FromComponentInNewPrefab(_enemyPanelPrefab)
            .UnderTransform(_levelUICanvas.transform)
            .AsSingle()
            .WithArguments(_spawnPoints.Count, _enemyIconPrefab)
            .NonLazy();
    }

    private void BindEnemies()
    {
        Container.Bind<List<EnemySpawnPoint>>().FromInstance(_spawnPoints).AsSingle();
        
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

    private void BindLevel()
    {
        Container.Bind<LevelStats>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Level>().AsSingle().WithArguments(_spawnPoints).NonLazy();
    }
}
