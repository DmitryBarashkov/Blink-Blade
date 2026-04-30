using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;

using static UnityEngine.Object;
using Zenject;

public class EffectsSpawner
{
    private ParticleSystem _teleportEffect;
    private ParticleSystem _trailTeleportEffect;
    
    private int _poolCapacity = 3;
    private int _poolMaxSize = 3;
    private int _lifetime = 1000;

    private float _shapeSize = 0.2f;
    private float _half = 0.5f;
    private int _emitAmount = 30;

    private ObjectPool<ParticleSystem> TeleportPool;      
    private ObjectPool<ParticleSystem> TrailTeleportPool;      

    [Inject]
    private void Construct(ParticleSystem teleportEffect, ParticleSystem trailEffect)
    {
        _teleportEffect = teleportEffect;
        _trailTeleportEffect = trailEffect;

        InitializePools();
    }

    private void InitializePools()
    {
        TeleportPool = new ObjectPool<ParticleSystem>(
        createFunc: () => Instantiate(_teleportEffect),
        actionOnGet: (obj) => obj.gameObject.SetActive(true),
        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);

        TrailTeleportPool = new ObjectPool<ParticleSystem>(
        createFunc: () => Instantiate(_trailTeleportEffect),
        actionOnGet: (obj) => obj.gameObject.SetActive(true),
        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    public void SpawnTeleportEffect(Transform spawnTransform)
    {
        ParticleSystem effect = TeleportPool.Get();

        effect.transform.SetParent(spawnTransform);
        
        effect.transform.localPosition = _teleportEffect.transform.position;
        effect.transform.localRotation = _teleportEffect.transform.rotation;
        
        effect.transform.SetParent(null);

        effect.Play();

        ReleaseOnLifetimeEnds(effect, TeleportPool).Forget();
    }

    public void SpawnTrailEffect(Vector3 start, Vector3 end)
    {
        ParticleSystem trailEffect = TrailTeleportPool.Get();

        trailEffect.Stop();
        trailEffect.Clear();

        Vector3 direction = end - start;
        Vector3 midPoint = start + direction * _half;
        float distance = direction.magnitude;
        var shape = _trailTeleportEffect.shape;

        shape.shapeType = ParticleSystemShapeType.Box;

        trailEffect.transform.position = midPoint;
        trailEffect.transform.forward = direction.normalized;

        shape.scale = new Vector3(_shapeSize, _shapeSize, distance);

        trailEffect.Emit(_emitAmount * Mathf.RoundToInt(distance));

        ReleaseOnLifetimeEnds(trailEffect, TrailTeleportPool).Forget();
    }

    private async UniTask ReleaseOnLifetimeEnds(ParticleSystem effect, ObjectPool<ParticleSystem> pool)
    {
        await UniTask.Delay(_lifetime);

        if (effect != null)
        {
            pool.Release(effect);
        }
    }
}