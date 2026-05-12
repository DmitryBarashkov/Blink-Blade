using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyWeapon : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            player.Die(collision.contacts[0]);            
        }
    }
}
