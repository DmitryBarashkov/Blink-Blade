using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnPointGetter: MonoBehaviour
{
    public List<EnemySpawnPoint> GetSpawnPoints() => GetComponentsInChildren<EnemySpawnPoint>(true).ToList<EnemySpawnPoint>();
}
