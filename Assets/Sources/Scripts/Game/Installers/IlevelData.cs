using System.Collections.Generic;
using BlinkBlade.Props;
using BlinkBlade.UI;
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

        bool IsTutorialLevel();

        UIScreen GetTutorialScreen();

        ParticleSystem GetMovingEffect();

        SoundType GetAmbientSoundType();
    }
}