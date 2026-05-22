using UnityEngine;
using Zenject;

public class HitEffectSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private SoundType _sound;

    [Inject] private IAudioService _audioService;

    public void Perform(ContactPoint hitPoint)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.forward, hitPoint.normal);

        Instantiate(_hitEffect, hitPoint.point, spawnRotation);

        _audioService.PlaySound(_sound);
    }
}
