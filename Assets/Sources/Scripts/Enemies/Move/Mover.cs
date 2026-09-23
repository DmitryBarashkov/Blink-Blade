using System;
using BlinkBlade.Enemies;
using BlinkBlade.Game;
using UnityEngine;

namespace BlinkBlade
{
    public abstract class Mover : MonoBehaviour
    {
        protected Transform _transform;
        protected EnemyAnimator _animator;
        protected bool _isActive = true;

        public event Action MovementStarted;

        public virtual void Initialize(
            Transform transform,
            CapsuleCollider collider,
            EnemyAnimator animator,
            ILevelData levelData,
            IAudioService audioService)
        {
            _transform = transform;
            _animator = animator;
        }

        public abstract void Activate();

        public abstract void Move();

        public abstract void KeepMoving();

        public abstract void Stop();

        public abstract void Deactivate();

        protected void InvokeMovementStarted()
        {
            MovementStarted?.Invoke();
        }
    }
}
