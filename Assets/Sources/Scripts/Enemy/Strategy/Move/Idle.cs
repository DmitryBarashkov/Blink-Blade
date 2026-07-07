using System;
using UnityEngine;

[Serializable]
public class Idle : IMovementStrategy
{
    public event Action MovementStarted = delegate { };
    public void Initialize(Transform transform, CapsuleCollider collider, EnemyAnimator animator, 
                           ILevelData levelData, IAudioService audioService, 
                           float wallCheckDistance, float cliffForwardOffset) { }
    public void Activate() { }
    public void Tick() { }
    public void Perform() { }
    public void KeepMoving() { }
    public void Deactivate() { }
    public void Stop() { }
}
