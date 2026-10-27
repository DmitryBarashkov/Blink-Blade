using BlinkBlade.Game;
using UnityEngine;

using static BlinkBlade.Game.GroundTypesDatabase;

namespace BlinkBlade.Props
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private GroundType _type;
        [SerializeField] private GroundTypesDatabase _database;

        private float _bounceForce;

        public float BounceForce => _bounceForce;

        private void Awake()
        {
            if (_database.TryGetGroundType(_type, out GroundTypeRecord result))
                _bounceForce = result.BounceForce;
        }
    }
}