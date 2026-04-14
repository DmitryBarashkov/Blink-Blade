using UnityEngine;

public class Target : MonoBehaviour
{
    private Transform _transform;

    private float _fixedZ = 0;

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        if (transform.position.z != _fixedZ)
            Utils.FixPositionZ(_transform, _fixedZ);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
