using System;
using BlinkBlade.Game;
using BlinkBlade.Players;
using UnityEngine;
using Zenject;

namespace BlinkBlade.Enemies
{
    [RequireComponent(typeof(Collider))]
    public abstract class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] protected EnemyWeapon _weapon;

        protected Collider _collider;
        protected LayerMask _layerMask;
        protected EnemyAnimator _animator;
        protected IAudioService _audioService;
        protected ObjectPoolService _poolService;
        protected float _cooldownTimer = 0;
        protected bool _isActive;

        public event Action<Player> PlayerEnteredAttackArea;

        public event Action PlayerLeftAttackArea;

        public event Action AttackStarted;

        public event Action AttackStopped;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _layerMask = LayerMask.GetMask("Player");
        }

        public virtual void Initialize(IAudioService audioService, EnemyAnimator animator, ObjectPoolService poolService, Enemy enemy)
        {
            _audioService = audioService;
            _animator = animator;
            _poolService = poolService;
        }

        public abstract void Activate();

        public abstract void Deactivate();

        protected abstract void Attack(Player player);

        protected abstract void StopAttack();

        protected void InvokeAttackStarted()
        {
            AttackStarted?.Invoke();
        }

        protected void InvokeAttackStopped()
        {
            AttackStopped?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player && player.IsInvincible == false)
            {
                Attack(player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player && player.IsInvincible == false)
            {
                StopAttack();
            }
        }
    }
}