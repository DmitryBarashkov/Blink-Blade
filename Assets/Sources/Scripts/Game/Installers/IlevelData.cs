using System.Collections.Generic;
using UnityEngine;

public interface ILevelData
{
    IReadOnlyList<EnemySpawnPoint> GetEnemySpawnPoints();

    EnemySpawnPoint GetCurrentEnemySpawnPoint(Transform spawnPointTransform);

    PlayerSpawnPoint GetPlayerSpawnPoint();

    CameraBounds GetCameraBounds();

    IReadOnlyList<ArrowTrap> GetArrowTraps();

    bool IsBossLevel();

    int GetBossHealth();

    ParticleSystem GetMovingEffect();

    SoundType GetAmbientSoundType();
}
