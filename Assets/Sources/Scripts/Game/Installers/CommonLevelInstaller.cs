using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

public class CommonLevelInstaller : MonoInstaller
{
    [Header("UI")]
    [SerializeField] private EnemyPanel _enemyPanelPrefab;    
    [SerializeField] private AssetReference _winGameScreen;
    [SerializeField] private AssetReference _loseGameScreen;
    [SerializeField] private AssetReference _shopGameScreen;
    [SerializeField] private CanvasScaler[] _canvasScales;

    [Header("Services")]
    [SerializeField] private AudioService _audioServicePrefab;
    [SerializeField] private ObjectPoolService _objectPoolServicePrefab;

    [Header("Containers")]
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _serviceContainer;
    [SerializeField] private Transform _poolContainer;
    [SerializeField] private Transform _levelUIContainer;
    [SerializeField] private Transform _endGameContainer;  
    [SerializeField] private Transform _betweenLevelContainer;  

    public override void InstallBindings()
    {
        InstallServices();
        BindEnemies();
        BindLevelServices();
        BindEnemiesUI();
        BindUIServices();        
    }

    private void InstallServices()
    {
        Container.BindInterfacesAndSelfTo<AudioService>()
            .FromComponentInNewPrefab(_audioServicePrefab)
            .UnderTransform(_serviceContainer)
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<ObjectPoolService>()
            .FromComponentInNewPrefab(_objectPoolServicePrefab)
            .UnderTransform(_serviceContainer)
            .AsSingle()
            .WithArguments(_poolContainer)
            .NonLazy();
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
            .UnderTransform(_levelUIContainer)
            .AsSingle()
            .NonLazy();
    }

    private void BindUIServices()
    {
        Container.BindInterfacesAndSelfTo<UIService>().AsSingle().WithArguments(_winGameScreen, _loseGameScreen, _shopGameScreen, _endGameContainer, _betweenLevelContainer).NonLazy();
        Container.Bind<CanvasScaleAdapter>().AsSingle().WithArguments(_canvasScales).NonLazy();
        Container.BindInterfacesAndSelfTo<ScreenResolutionAdapter>().AsSingle().NonLazy();

        Container.BindFactory<Transform, AssetReference, IObservable<UIScreen>, UIScreen.Factory>()
            .FromMethod((container, parent, reference) =>
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(reference);

                return handle.Task.ToObservable()
                        .ObserveOnMainThread()
                        .Select(prefab =>
                        {
                            GameObject go = container.InstantiatePrefab(prefab, parent);

                            return go.GetComponent<UIScreen>();
                        });
            });
    }
}