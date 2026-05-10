using UnityEngine;

public class EnemySpawnPointGetter: MonoBehaviour
{
    public EnemySpawnPoint[] GetSpawnPoints() => GetComponentsInChildren<EnemySpawnPoint>(true);
}
