using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MeleeWeapon : EnemyWeapon
{
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public override void Activate()
    {
        _collider.enabled = true;
    }

    public override void Deactivate()
    {
        _collider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null && player.IsInvincible == false)
        {
            player.Die(collision.contacts[0]);
        }
    }
}
