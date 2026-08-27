using System;
using UnityEngine;

[Serializable]
public class Idle : IMovementStrategy
{
    public event Action MovementStarted = () => { };

    public void Initialize(
        Transform transform,
        CapsuleCollider collider,
        EnemyAnimator animator,
        ILevelData levelData,
        IAudioService audioService,
        float wallCheckDistance,
        float cliffForwardOffset)
    {
        // Ignore
    }

    public void Activate()
    {
        // Ignore
    }

    public void Tick()
    {
        // Ignore
    }

    public void Perform()
    {
        // Ignore
    }

    public void KeepMoving()
    {
        // Ignore
    }

    public void Deactivate()
    {
        // Ignore
    }

    public void Stop()
    {
        // Ignore
    }
}
