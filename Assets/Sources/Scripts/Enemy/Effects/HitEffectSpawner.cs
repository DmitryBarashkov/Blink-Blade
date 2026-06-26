using UnityEngine;
using Zenject;
using static ObjectPoolService;

public class HitEffectSpawner : MonoBehaviour
{
    [SerializeField] private PoolObjectTypes _spawnObjectType;
    [SerializeField] private SoundType _sound;

    private IAudioService _audioService;
    private ObjectPoolService _objectPoolService;

    [Inject]
    public void Construct(IAudioService audioService, ObjectPoolService objectPoolService)
    {
        _audioService = audioService;
        _objectPoolService = objectPoolService;
    }

    public void Perform(ContactPoint hitPoint)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.forward, hitPoint.normal);

        _objectPoolService.Get(_spawnObjectType, hitPoint.point, spawnRotation);

        _audioService.PlaySound(_sound);
    }
}
