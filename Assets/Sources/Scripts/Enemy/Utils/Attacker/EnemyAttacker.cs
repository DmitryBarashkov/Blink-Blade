using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class EnemyAttacker : MonoBehaviour
{
    public event Action<Player> OnPlayerInAttackArea;
    public event Action OnPlayerOutAttackArea;
    
    [SerializeField] protected EnemyWeapon _weapon;

    protected Collider _collider;

    protected LayerMask _layerMask;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _layerMask = LayerMask.GetMask("Player");
    }

    public abstract void Activate();
    
    public abstract void Deactivate();

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        
        if (player && player.IsInvincible == false)
        {
            TriggerAttack(player);
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

    protected void TriggerAttack(Player player)
    {
        OnPlayerInAttackArea?.Invoke(player);
    }
}
