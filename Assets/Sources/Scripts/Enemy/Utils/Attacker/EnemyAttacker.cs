using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class EnemyAttacker : MonoBehaviour
{
    public event Action OnPlayerInAttackArea;
    public event Action OnPlayerOutAttackArea;
    
    [SerializeField] protected EnemyWeapon _weapon;
    
    protected Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public abstract void Activate();

    public abstract void Deactivate();

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
