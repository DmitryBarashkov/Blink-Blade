using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] Enemy _enemy;
    [SerializeField] EnemyWeapon _weapon;

    private BoxCollider _collider;
    private BoxCollider _weaponCollider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _weaponCollider = _weapon.GetComponent<BoxCollider>();
    }

    public void Enable()
    {
        _collider.enabled = true;
        _weaponCollider.enabled = true;
    }

    public void Disable()
    {
        _collider.enabled = false;
        _weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _enemy.Attack();
        }
    }
}
