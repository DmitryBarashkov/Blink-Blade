using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IDefendingStrategy
{
    event Action StartBlocking;
    
    void Initialize(Animator animator, RigBuilder rigBuilder, Blocker blocker, Shield shield);
    void Activate();
    void Deactivate();
    void StopBlock();
}
