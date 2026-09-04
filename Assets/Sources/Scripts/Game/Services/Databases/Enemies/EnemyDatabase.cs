using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Config/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    [Header("Assembled Enemies")]
    public List<AssembledEnemy> Enemies;

    public bool TryGetEnemy(string name, out AssembledEnemy result)
    {
        foreach (var enemy in Enemies)
        {
            if (enemy.EnemyName == name)
            {
                result = enemy;
                return true;
            }
        }

        result = default;
        return false;
    }

    [Serializable]
    public struct AssembledEnemy
    {
        public string EnemyName;
        public Enemy Prefab;
        [SerializeReference] public IMovementStrategy MovementStrategy;
        [SerializeReference] public IAttackingStrategy AttackingStrategy;
        [SerializeReference] public IDefendingStrategy DefendingStrategy;
    }
}