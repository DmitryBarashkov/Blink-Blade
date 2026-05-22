using UnityEngine;
using Zenject;

[RequireComponent(typeof(HitEffectSpawner))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IResetable
{
    [SerializeField] EnemyAttacker _attacker;

    protected Transform _transform;
    protected CapsuleCollider _collider;    
    
    private EnemyAnimator _animator;    
    private Vector3 _initiatePosition;
    private Quaternion _initiateRotation;

    private LevelState _levelState;
    private IAudioService _audioService;

    protected virtual EnemyAnimator AnimatorInstance => _animator;

    [Inject]
    public virtual void Construct(IAudioService audioService, LevelState levelState)
    {
        _transform = transform;        
        _audioService = audioService;
        _levelState = levelState;
    }

    protected virtual void Awake()
    {
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _collider = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnEnable()
    {
        _attacker.OnPlayerInAttackArea += Attack;
        _attacker.OnPlayerOutAttackArea += StopAttack;
    }

    protected virtual void OnDisable()
    {
        _attacker.OnPlayerInAttackArea -= Attack;
        _attacker.OnPlayerOutAttackArea -= StopAttack;
    }

    public virtual void Activate()
    {
        _transform.position = _initiatePosition;
        _transform.rotation = _initiateRotation;

        _attacker.Enable();

        AnimatorInstance.SetDied(false);
        _collider.enabled = true;
    }
    
    public virtual void Attack()
    {
        AnimatorInstance.SetAttack();
        _audioService.PlaySound(SoundType.SwordAttack);
    }

    public virtual void StopAttack()
    {

    }

    public virtual void Die(ContactPoint hitPoint)
    {
        _attacker.Disable();

        AnimatorInstance.SetDied(true);
        _collider.enabled = false;

        _levelState.CurrentEnemiesCount.Value--;
    }

    public void ResetOnRestart()
    {
        Activate();
    }

    public void SetInitiatePosition(Transform initTransform, Transform container)
    {
        _transform.SetParent(container);
        _transform.position = _initiatePosition = initTransform.position;
        _transform.rotation = _initiateRotation = initTransform.rotation;
    }

    public class Factory : PlaceholderFactory<Object, Enemy> { }
}
