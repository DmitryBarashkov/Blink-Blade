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
        Player player = other.GetComponent<Player>();
        
        if (player && player.IsInvincible == false)
        {
            OnPlayerInAttackArea?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        
        if (player && player.IsInvincible == false)
        {
            OnPlayerOutAttackArea?.Invoke();
        }
    }
}
