using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller, ILevelData
{
    [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
    [SerializeField] private List<EnemySpawnPoint> _enemySpawnPoints;
    [SerializeField] private List<ArrowTrap> _arrowTraps;
    [SerializeField] private CameraBounds _bounds;
    [SerializeField] private SoundType _ambientSoundType = SoundType.ForestAmbientSounds;

    [Header("Boss Level Settings")]
    [SerializeField] private ParticleSystem _movingEffect;
    [SerializeField] private bool _isBossLevel = false;
    [SerializeField] private int _bossHealth = 3;

    [Inject] private LevelBridge _levelBridge;

    public override void InstallBindings()
    {
        Container.Bind<CameraBounds>().FromComponentsInHierarchy().AsSingle();

        Container.Bind<HitEffectSpawner>()
            .FromComponentsInHierarchy()
            .AsCached();

        _levelBridge.CurrentLevel.Value = this;
    }

    public IReadOnlyList<EnemySpawnPoint> GetEnemySpawnPoints() => _enemySpawnPoints;
    public PlayerSpawnPoint GetPlayerSpawnPoint() => _playerSpawnPoint;

    public EnemySpawnPoint GetCurrentEnemySpawnPoint(Transform spawnPointTransform)
    {
        foreach (var spawnPoint in _enemySpawnPoints)
        {
            if (spawnPoint.transform == spawnPointTransform)
                return spawnPoint;
        }

        return null;
    }

    public IReadOnlyList<ArrowTrap> GetArrowTraps() => _arrowTraps;
    public CameraBounds GetCameraBounds() => _bounds;
    public bool IsBossLevel() => _isBossLevel;
    public int GetBossHealth() => _bossHealth;
    public ParticleSystem GetMovingEffect() => _movingEffect;
    public SoundType GetAmbientSoundType() => _ambientSoundType;

    private void OnDestroy()
    {
        if (_levelBridge != null)
        {
            _levelBridge.CurrentLevel.Value = null;
        }
    }
}
