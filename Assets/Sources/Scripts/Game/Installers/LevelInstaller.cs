using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private EnemySpawnPointGetter _spawnPointsGetter;

    [Header("UI")]
    [SerializeField] private EnemyPanel _enemyPanelPrefab;
    [SerializeField] private EnemyIcon _enemyIconPrefab;
    [SerializeField] private Canvas _levelUICanvas;
    [SerializeField] private Canvas _endGameCanvas;
    [SerializeField] private AssetReference _winGameScreen;
    [SerializeField] private AssetReference _loseGameScreen;

    private EnemySpawnPoint[] _spawnPoints;

    public override void InstallBindings()
    {
        _spawnPoints = _spawnPointsGetter.GetSpawnPoints();
        
        BindEnemies();
        BindLevelServices();
        BindEnemiesUI();
        BindEndGameUI();
    }

    private void BindEnemies()
    {
        foreach (var spawnPoint in _spawnPoints)
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
        }
    }

    private void BindLevelServices()
    {
        Container.BindFactory<LevelRestartService, LevelRestartService.Factory>().AsSingle();        
        Container.Bind<LevelState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Level>().AsSingle().WithArguments(_spawnPoints.Length).NonLazy();
    }

    private void BindEnemiesUI()
    {
        Container.BindInterfacesAndSelfTo<EnemyPanel>()
            .FromComponentInNewPrefab(_enemyPanelPrefab)
            .UnderTransform(_levelUICanvas.transform)
            .AsSingle()
            .WithArguments(_spawnPoints.Length, _enemyIconPrefab)
            .NonLazy();
    }

    private void BindEndGameUI()
    {
        Container.Bind<EndGameScreen.Factory>().AsSingle().WithArguments(_endGameCanvas.transform);        
        Container.BindInterfacesTo<UIService>().AsSingle().WithArguments(_winGameScreen, _loseGameScreen);
        
    }
}
