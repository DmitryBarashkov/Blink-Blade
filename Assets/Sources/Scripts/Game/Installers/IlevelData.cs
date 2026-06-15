using System.Collections.Generic;

public interface ILevelData
{
    List<EnemySpawnPoint> GetEnemySpawnPoints();
    PlayerSpawnPoint GetPlayerSpawnPoint();

    CameraBounds GetCameraBounds();
}
