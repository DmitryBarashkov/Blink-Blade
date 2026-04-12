using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collided with {collision.collider.name}");
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
