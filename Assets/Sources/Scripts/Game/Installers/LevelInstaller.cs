using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private List<EnemySpawnPoint> _spawnPoints;

    [Header("UI")]
    [SerializeField] private EnemyPanel _enemyPanelPrefab;
    [SerializeField] private EnemyIcon _enemyIconPrefab;
    [SerializeField] private Canvas _levelUICanvas;
    [SerializeField] private Canvas _endGameCanvas;
    [SerializeField] private AssetReference _winGameWindow;
    [SerializeField] private AssetReference _loseGameWindow;

    public override void InstallBindings()
    {
        BindEnemies();
        BindLevel();
        BindEnemiesUI();
        BindEndGameUI();
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
                .AsCached()
                .WithArguments(spawnPoint.transform)
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
        Container.Bind<LevelRestartService>().AsSingle();
        Container.Bind<LevelState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Level>().AsSingle().WithArguments(_spawnPoints.Count).NonLazy();
    }

    private void BindEndGameUI()
    {
        Container.Bind<EndGameWindow.Factory>().AsSingle().WithArguments(_endGameCanvas.transform);        
        Container.BindInterfacesTo<UIService>().AsSingle().WithArguments(_winGameWindow, _loseGameWindow);
        
    }
}
