using UnityEngine;
using Zenject;
using static EnemyDatabase;

public class EnemyFactory
{
    private readonly DiContainer _container;
    private readonly EnemyDatabase _database;

    public EnemyFactory(DiContainer container, EnemyDatabase database)
    {
        _container = container;
        _database = database;
    }

    public Enemy Create(EnemySpawnPoint spawnPoint, Transform enemyContainer)
    {
        string enemyName = spawnPoint.selectedEnemyName;
        Transform enemyTransform = spawnPoint.transform;
        
        if (_database.TryGetEnemy(enemyName, out AssembledEnemy enemyRecord))
        {
            Enemy enemy = _container.InstantiatePrefabForComponent<Enemy>(
                enemyRecord.prefab,
                enemyTransform.position,
                enemyTransform.rotation,
                enemyContainer,
                GetClonedStrategies(enemyRecord));

            return enemy;
        }

        Debug.Log($"Не удалось создать врага {enemyName}");
        return null;
    }

    private object[] GetClonedStrategies(AssembledEnemy enemyRecord)
    {
        var originalMovement = enemyRecord.movementStrategy;
        string moveJson = JsonUtility.ToJson(originalMovement);
        IMovementStrategy clonedMovement = JsonUtility.FromJson(moveJson, originalMovement.GetType()) as IMovementStrategy;
        
        var originalAttacking = enemyRecord.attackingStrategy;
        string attackJson = JsonUtility.ToJson(originalAttacking);
        IAttackingStrategy clonedAttack = JsonUtility.FromJson(attackJson, originalAttacking.GetType()) as IAttackingStrategy;

        var originalDefending = enemyRecord.defendingStrategy;
        string defendJson = JsonUtility.ToJson(originalAttacking);
        IDefendingStrategy cloneddefend = JsonUtility.FromJson(defendJson, originalDefending.GetType()) as IDefendingStrategy;

        return new object[] { clonedMovement, clonedAttack, cloneddefend };
    }
}
