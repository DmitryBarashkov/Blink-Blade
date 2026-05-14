using UnityEngine;

public class HitEffectSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;    

    public void Perform(ContactPoint hitPoint)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.forward, hitPoint.normal);

        Instantiate(_hitEffect, hitPoint.point, spawnRotation);
    }
}
