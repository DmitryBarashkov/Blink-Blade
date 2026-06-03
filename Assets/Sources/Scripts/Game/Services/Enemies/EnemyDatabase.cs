using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Config/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    [Serializable]
    public struct AssembledEnemy
    {
        public string enemyName;
        public Enemy prefab;
        [SerializeReference] public IMovementStrategy movementStrategy;
        [SerializeReference] public IAttackingStrategy attackingStrategy;
        [SerializeReference] public IDefendingStrategy defendingStrategy;
    }

    [Header("Assembled Enemies")]
    public List<AssembledEnemy> enemies;

    public bool TryGetEnemy(string name, out AssembledEnemy result)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.enemyName == name)
            {
                result = enemy;
                return true;
            }
        }

        result = default;
        return false;
    }
}