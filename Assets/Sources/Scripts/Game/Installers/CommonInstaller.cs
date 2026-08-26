using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CommonInstaller : MonoInstaller
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _camera;

    [Header("UI")]
    [SerializeField] private EnemyPanel _enemyPanelPrefab;
    [SerializeField] private UIScreen _winGameScreen;
    [SerializeField] private UIScreen _loseGameScreen;
    [SerializeField] private UIScreen _shopGameScreen;
    [SerializeField] private UIScreen _finishGameScreen;
    [SerializeField] private CanvasScaler[] _canvasScales;

    [Header("Services")]
    [SerializeField] private ObjectPoolService _objectPoolServicePrefab;

    [Header("Containers")]
    [SerializeField] private Transform _levelContainer;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _serviceContainer;
    [SerializeField] private Transform _poolContainer;
    [SerializeField] private Transform _endGameContainer;
    [SerializeField] private Transform _betweenLevelContainer;

    public override void InstallBindings()
    {
        BindServices();
        BindCamera();
        BindEnemies();
        BindEnemiesUIPrefabs();
        BindSpawners();
        BindLevelServices();
        BindUIServices();
    }

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<ShopService>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<ObjectPoolService>()
            .FromComponentInNewPrefab(_objectPoolServicePrefab)
            .UnderTransform(_serviceContainer)
            .AsSingle()
            .WithArguments(_poolContainer)
            .NonLazy();
    }

    private void BindCamera()
    {
        Container.Bind<CameraResizer>().AsSingle().WithArguments(_camera).NonLazy();
        Container.BindInterfacesAndSelfTo<CameraBoundsInstaller>().AsSingle().WithArguments(_camera).NonLazy();
        Container.BindInterfacesAndSelfTo<CameraOffsetChanger>().AsSingle().WithArguments(_camera).NonLazy();
    }

    private void BindEnemiesUIPrefabs()
    {
        Container.BindInterfacesAndSelfTo<EnemyPanel>()
            .FromComponentInNewPrefab(_enemyPanelPrefab)
            .UnderTransform(_levelContainer)
            .AsSingle()
            .NonLazy();
    }

    private void BindEnemies()
    {
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<Patrol>().AsTransient();
    }

    private void BindLevelServices()
    {
        Container.BindInterfacesAndSelfTo<LevelScreen>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Level>().AsSingle().NonLazy();
    }

    private void BindSpawners()
    {
        Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().WithArguments(_enemyContainer).NonLazy();
    }

    private void BindUIServices()
    {
        Container.BindInterfacesAndSelfTo<UIService>()
            .AsSingle()
            .WithArguments(_winGameScreen, _loseGameScreen, _finishGameScreen, _shopGameScreen, _endGameContainer, _betweenLevelContainer)
            .NonLazy();
        Container.Bind<CanvasScaleAdapter>().AsSingle().WithArguments(_canvasScales).NonLazy();
        Container.BindInterfacesAndSelfTo<ScreenResolutionAdapter>().AsSingle().NonLazy();
    }
}