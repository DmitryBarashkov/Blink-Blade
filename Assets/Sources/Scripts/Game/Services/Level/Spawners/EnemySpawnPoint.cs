using BlinkBlade.Enemies;
using UnityEngine;

namespace BlinkBlade.Game
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;

        public Enemy Enemy => _enemy;
    }
}