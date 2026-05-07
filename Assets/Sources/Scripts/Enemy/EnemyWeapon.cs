using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyWeapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.Die();            
        }
    }
}
