using System;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : IMovementStrategy
{
    public event Action MovementStarted = delegate { };

    private Transform _transform;
    private ILevelData _levelData;
    private IAudioService _audioService;
    private ParticleSystem _effect;
    private int _pointIndex;
    private EnemySpawnPoint _currentPoint;
    private EnemyAnimator _animator;
    private List<EnemySpawnPoint> _spawnPoints;
    private float _height;

    public void Initialize(Transform transform, CapsuleCollider collider, EnemyAnimator animator, 
                           ILevelData levelData, IAudioService audioService, 
                           float wallCheckDistance, float cliffForwardOffset)
    {
        _transform = transform;
        
        _levelData = levelData;
        _audioService = audioService;
        _animator = animator;
        
        _effect = levelData.GetMovingEffect();

        _height = collider.height;
    }

    public void Activate()
    {
        _pointIndex = 0;
        _spawnPoints = _levelData.GetEnemySpawnPoints() as List<EnemySpawnPoint>;
        _currentPoint = _spawnPoints[_pointIndex];

        MoveToPosition();

        _animator.SetCast();
        _audioService.PlaySound(SoundType.CastScream);
    }

    public void Deactivate()
    {
        _currentPoint = null;
        _spawnPoints = null;
    }

    public void KeepMoving() { }

    public void Perform()
    {
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

    public void Stop() { }

    public void Tick() { }

    private void MoveToPosition()
    {
        _transform.position = _currentPoint.transform.position;
        _transform.rotation = _currentPoint.transform.rotation;
    }
}
