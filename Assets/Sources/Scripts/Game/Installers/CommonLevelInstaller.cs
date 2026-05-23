using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

public class CommonLevelInstaller : MonoInstaller
{
    [Header("UI")]
    [SerializeField] private EnemyPanel _enemyPanelPrefab;    
    [SerializeField] private Canvas _levelUICanvas;
    [SerializeField] private Canvas _endGameCanvas;    
    [SerializeField] private AssetReference _winGameScreen;
    [SerializeField] private AssetReference _loseGameScreen;
    [SerializeField] private CanvasScaler[] _canvasScales;

    [Header("Containers")]
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _effectsContainer;

    public override void InstallBindings()
    {
        BindEnemies();
        BindLevelServices();
        BindEnemiesUI();
        BindUIServices();        
    }

    private void BindEnemies()
    {
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<Patrol>().AsTransient();
        Container.Bind<EnemySpawnPoint>().FromComponentsInHierarchy().AsCached();
        Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().WithArguments(_enemyContainer).NonLazy();
    }

    private void BindLevelServices()
    {
        Container.Bind<IResetable>().FromComponentsInHierarchy().AsCached();
        Container.BindFactory<LevelRestartService, LevelRestartService.Factory>().AsSingle();        
        Container.Bind<LevelState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Level>().AsSingle().NonLazy();
    }

    private void BindEnemiesUI()
    {
        Container.BindInterfacesAndSelfTo<EnemyPanel>()
            .FromComponentInNewPrefab(_enemyPanelPrefab)
            .UnderTransform(_levelUICanvas.transform)
            .AsSingle()
            .NonLazy();
    }

    private void BindUIServices()
    {
        Container.Bind<EndGameScreen.Factory>().AsSingle().WithArguments(_endGameCanvas.transform);        
        Container.BindInterfacesTo<UIService>().AsSingle().WithArguments(_winGameScreen, _loseGameScreen);
        Container.Bind<CanvasScaleAdapter>().AsSingle().WithArguments(_canvasScales).NonLazy();
        Container.BindInterfacesAndSelfTo<ScreenResolutionAdapter>().AsSingle().NonLazy();        
    }
}