using BlinkBlade.Game;
using BlinkBlade.Players;
using UnityEngine;

namespace BlinkBlade.Enemies
{
    public class MeleeAttacker : EnemyAttacker
    {
        [SerializeField] private float _cooldown = 1f;

        private void Update()
        {
            if (_isActive)
            {
                _cooldownTimer += Time.deltaTime;

                if (_cooldownTimer >= _cooldown)
                {
                    StopAttack();
                }
            }
        }

        public override void Activate()
        {
            _collider.enabled = true;
            _weapon.Activate();
        }

        public override void Deactivate()
        {
            _collider.enabled = false;
            _weapon.Deactivate();
        }

        protected override void Attack(Player player)
        {
            _animator.SetMeleeAttack();
            _audioService.PlaySound(SoundType.SwordAttack);
            _isActive = true;

            InvokeAttackStarted();
        }

        protected override void StopAttack()
        {
            InvokeAttackStopped();
            _isActive = false;
            _cooldownTimer = 0;
        }
    }
}