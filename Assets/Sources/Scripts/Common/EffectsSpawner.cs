using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;

using static UnityEngine.Object;

public class EffectsSpawner
{
    private ParticleSystem _startTeleportEffect;
    private ParticleSystem _trailTeleportEffect;
    
    private int _poolCapacity = 5;
    private int _poolMaxSize = 5;
    private int _lifetime = 1000;

    private float _shapeSize = 0.2f;
    private float _half = 0.5f;
    private int _emitAmount = 20;

    private ObjectPool<ParticleSystem> Pool;        

    public EffectsSpawner(ParticleSystem startEffect, ParticleSystem trailEffect)
    {
        _startTeleportEffect = startEffect;
        _trailTeleportEffect = trailEffect;

        InitializePool();
    }

    private void InitializePool()
    {
        Pool = new ObjectPool<ParticleSystem>(
        createFunc: () => Instantiate(_startTeleportEffect),
        actionOnGet: (obj) => obj.gameObject.SetActive(true),
        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    public void SpawnEffectFromPool(Transform spawnTransform)
    {
        ParticleSystem effect = Pool.Get();

        effect.transform.SetParent(spawnTransform);
        effect.transform.localPosition = _startTeleportEffect.transform.position;
        effect.transform.localRotation = _startTeleportEffect.transform.rotation;
        effect.transform.SetParent(null);

        effect.Play();

        ReleaseOnLifetimeEnds(effect).Forget();
    }

    public void SpawnTrailEffect(Vector3 startLinePosition, Vector3 endLinePosition)
    {
        Vector3 direction = endLinePosition - startLinePosition;
        Vector3 midPoint = startLinePosition + direction * _half;
        var shape = _trailTeleportEffect.shape;

        shape.shapeType = ParticleSystemShapeType.Box;

        _trailTeleportEffect.transform.position = midPoint;
        _trailTeleportEffect.transform.forward = direction.normalized;

        shape.scale = new Vector3(_shapeSize, _shapeSize, direction.magnitude);

        _trailTeleportEffect.Emit(_emitAmount * Mathf.RoundToInt(direction.magnitude));
    }

    private async UniTask ReleaseOnLifetimeEnds(ParticleSystem obj)
    {
        await UniTask.Delay(_lifetime);

        if (obj != null)
        {
            ReleaseEffect(obj);
        }
    }

    private void ReleaseEffect(ParticleSystem obj)
    {
        Pool.Release(obj);
    }
}