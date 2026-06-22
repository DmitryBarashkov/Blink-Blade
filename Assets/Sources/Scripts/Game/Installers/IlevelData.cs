using System.Collections.Generic;

public interface ILevelData
{
    IReadOnlyList<EnemySpawnPoint> GetEnemySpawnPoints();
    PlayerSpawnPoint GetPlayerSpawnPoint();

    CameraBounds GetCameraBounds();
    IReadOnlyList<ArrowTrap> GetArrowTraps();
}
