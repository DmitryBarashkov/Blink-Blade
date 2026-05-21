using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Config/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    [System.Serializable]
    public struct IdleMapping 
    { 
        public EnemyType type;
        public BehaviourType behaviour;
        public Enemy prefab;
    }

    [System.Serializable]
    public struct PatrolMapping
    {
        public EnemyType type;
        public BehaviourType behaviour;
        public PatrolEnemy prefab;
    }

    [Header("Обычные враги")]
    public List<IdleMapping> idleEnemies;

    [Header("Патрульные враги")]
    public List<PatrolMapping> patrolEnemies;
}