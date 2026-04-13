using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    private void ChangePositionToObstacle(Collision collision)
    {
        Debug.Log($"Collided with {collision.collider.name}");

        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Vector3 impactDir = collision.relativeVelocity.normalized;

        _transform.forward = impactDir;
        _transform.position += _transform.forward * 0.2f;
        _transform.SetParent(collision.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ChangePositionToObstacle(collision);
    }

    private void OnDrawGizmosSelected()
    {
        if (_rigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(_rigidbody.centerOfMass), 0.05f);
        }
    }
}
