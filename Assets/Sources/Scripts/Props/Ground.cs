using UnityEngine;
using static GroundTypeDatabase;

public class Ground : MonoBehaviour
{
    [SerializeField] private GroundType _type;
    [SerializeField] private GroundTypeDatabase _database;

    private float _bounceForce;

    public float BounceForce => _bounceForce;

    private void Awake()
    {
        if (_database.TryGetGroundType(_type, out GroundTypeRecord result))
            _bounceForce = result.bounceForce;
    }
}
