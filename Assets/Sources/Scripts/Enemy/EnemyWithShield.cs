using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyWithShield : Enemy
{
    [SerializeField] private Blocker _blocker;
    [SerializeField] private Shield _shield;
    
    private RigBuilder _rigBuilder;
    private ShieldEnemyAnimator _animator;

    protected override EnemyAnimator AnimatorInstance => _animator;

    protected override void Awake()
    {
        _animator = new ShieldEnemyAnimator(GetComponent<Animator>());
        _collider = GetComponent<CapsuleCollider>();
        _rigBuilder = GetComponent<RigBuilder>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _blocker.OnWeaponInBlockingArea += StartBlock;
        _blocker.OnWeaponOutBlockingArea += StopBlock;

        _shield.OnHitWeapon += ShieldImpact;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _blocker.OnWeaponInBlockingArea -= StartBlock;
        _blocker.OnWeaponOutBlockingArea -= StopBlock;
    }

    public override void Die(ContactPoint hitPoint)
    {
        base.Die(hitPoint);

        _shield.Deactivate();
        _rigBuilder.enabled = false;
    }

    public override void Activate()
    {
        base.Activate();

        _shield.Activate();
        _rigBuilder.enabled = true;
    }

    private void StartBlock()
    {
        _animator.SetBlocking(true);
    }

    private void StopBlock()
    {
        _animator.SetBlocking(false);
    }

    private void ShieldImpact()
    {
        _animator.BlockImpact();
    }
}
