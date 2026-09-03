using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public class Defenceless : IDefendingStrategy
{
    public event Action StartBlocking = () => { };

    public void Initialize(Animator animator, RigBuilder rigBuilder, Blocker blocker, Shield shield)
    {
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public void StopBlock()
    {
    }
}
