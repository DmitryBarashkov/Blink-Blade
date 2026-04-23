using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;    

    public void Perform(ContactPoint hitPoint)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.forward, hitPoint.normal);

        ParticleSystem bloodBlow = Instantiate(_hitEffect, hitPoint.point, spawnRotation);
    }
}
