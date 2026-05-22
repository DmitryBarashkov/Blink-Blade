using UnityEngine;
using Zenject;

public class PatrolEnemy : Enemy
{
    [SerializeField] private float _wallCheckDistance;
    
    private Patrol _patrol;
    
    [Inject]
    public void ConstructPatrol(Patrol patrol)
    {
        _patrol = patrol;              
    }

    protected override void Awake()
    {
        base.Awake();
        _patrol.Initialize(_transform, AnimatorInstance, _wallCheckDistance);
    }

    private void Update()
    {
        _patrol.UpdateTick();
    }

    public override void Attack()
    {
        _patrol.Stop();
        base.Attack();
    }

    public override void StopAttack()
    {
        _patrol.KeepMoving();
    }

    public override void Die(ContactPoint hitPoint)
    {
        base.Die(hitPoint);

        _patrol.Stop();        
    }

    public override void Activate()
    {
        base.Activate();

        _patrol.Start();
    }
}
