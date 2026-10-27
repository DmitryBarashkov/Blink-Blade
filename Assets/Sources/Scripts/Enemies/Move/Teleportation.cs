using System.Collections.Generic;
using BlinkBlade.Game;
using UnityEngine;

namespace BlinkBlade.Enemies
{
    public class Teleportation : Mover
    {
        private ILevelData _levelData;
        private IAudioService _audioService;
        private ParticleSystem _effect;
        private int _pointIndex;
        private EnemySpawnPoint _currentPoint;
        private List<EnemySpawnPoint> _spawnPoints;
        private float _height;

        public override void Initialize(
            Transform transform,
            CapsuleCollider collider,
            EnemyAnimator animator,
            ILevelData levelData,
            IAudioService audioService)
        {
            base.Initialize(transform, collider, animator, levelData, audioService);

            _levelData = levelData;
            _audioService = audioService;

            _effect = levelData.GetMovingEffect();

            _height = collider.height;

            _pointIndex = 0;
            _spawnPoints = _levelData.GetEnemySpawnPoints() as List<EnemySpawnPoint>;
        }

        public override void Activate()
        {
            _isActive = true;

            _pointIndex = 0;
            _currentPoint = _spawnPoints[_pointIndex];

            MoveToPosition();

            _animator.SetCast();
            _audioService.PlaySound(SoundType.CastScream);
        }

        public override void Move()
        {
            if (_isActive == false)
                return;

            Vector3 centerPosition = Vector3.up * _height / 2;

            _pointIndex++;
            _currentPoint = _spawnPoints[_pointIndex];

            _effect.transform.position = _transform.position + centerPosition;
            _effect.Play();
            _audioService.PlaySound(SoundType.Teleport);

            MoveToPosition();

            _animator.SetCast();
            _audioService.PlaySound(SoundType.CastScream);
        }

        public override void KeepMoving()
        {
            _isActive = true;
        }

        public override void Stop()
        {
            _isActive = false;
        }

        public override void Deactivate()
        {
            _isActive = false;
        }

        private void MoveToPosition()
        {
            _transform.position = _currentPoint.transform.position;
            _transform.rotation = _currentPoint.transform.rotation;
        }
    }
}