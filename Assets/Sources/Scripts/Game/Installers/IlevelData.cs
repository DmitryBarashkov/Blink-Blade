using System.Collections.Generic;
using BlinkBlade.Props;
using UnityEngine;

namespace BlinkBlade.Game
{
    public interface ILevelData
    {
        IReadOnlyList<EnemySpawnPoint> GetEnemySpawnPoints();

        EnemySpawnPoint GetCurrentEnemySpawnPoint(Transform spawnPointTransform);

        PlayerSpawnPoint GetPlayerSpawnPoint();

        PolygonCollider2D GetCameraBounds();

        IReadOnlyList<ArrowTrap> GetArrowTraps();

        bool IsBossLevel();

        int GetBossHealth();

        ParticleSystem GetMovingEffect();

        SoundType GetAmbientSoundType();
    }
}