using System;
using UnityEngine;

public interface IMovementStrategy
{
    event Action MovementStarted;

    void Initialize(
        Transform transform,
        CapsuleCollider collider,
        EnemyAnimator animator,
        ILevelData level,
        IAudioService audioService,
        float wallCheckDistance,
        float cliffForwardOffset);

    void Activate();

    void Tick();

    void KeepMoving();

    void Perform();

    void Deactivate();

    void Stop();
}
