using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAttacker : MonoBehaviour
{
    public event Action OnPlayerInAttackArea;
    public event Action OnPlayerOutAttackArea;
    
    [SerializeField] EnemyWeapon _weapon;

    private BoxCollider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();        

        if (_weapon == null)
            throw new ArgumentNullException(nameof(_weapon));
    }

    public void Activate()
    {
        _collider.enabled = true;
        _weapon.Activate();
    }

    public void Deactivate()
    {
        _collider.enabled = false;
        _weapon.Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            OnPlayerInAttackArea?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            OnPlayerOutAttackArea?.Invoke();
        }
    }
}
