using BlinkBlade.Players;
using UnityEngine;

namespace BlinkBlade.Props
{
    public class SpikeTrap : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Player player = collision.collider.GetComponent<Player>();

            ContactPoint hitPoint = collision.contacts[0];

            if (player != null && player.IsInvincible == false)
                player.Die(hitPoint);
        }
    }
}