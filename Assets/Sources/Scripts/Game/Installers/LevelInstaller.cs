using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller, ILevelData
{
    [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
    [SerializeField] private List<EnemySpawnPoint> _enemySpawnPoints;
    [SerializeField] private CameraBounds _bounds;

    [Inject] private ActiveLevelBridge _levelBridge;

    public override void InstallBindings()
    {
        Container.Bind<CameraBounds>().FromComponentsInHierarchy().AsSingle();

        Container.Bind<HitEffectSpawner>()
            .FromComponentsInHierarchy()
            .AsCached();

        _levelBridge.CurrentLevel.Value = this;
    }

    public List<EnemySpawnPoint> GetEnemySpawnPoints() => _enemySpawnPoints;
    public PlayerSpawnPoint GetPlayerSpawnPoint() => _playerSpawnPoint;
    public CameraBounds GetCameraBounds() => _bounds;

    private void OnDestroy()
    {
        if (_levelBridge != null)
        {
            _levelBridge.CurrentLevel.Value = null;
        }
    }
}
