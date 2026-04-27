using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;

using static UnityEngine.Object;
using Zenject;
using System;

public class EffectsSpawner
{
    private ParticleSystem _startTeleportEffect;
    private ParticleSystem _trailTeleportEffect;
    
    private int _poolCapacity = 3;
    private int _poolMaxSize = 3;
    private int _lifetime = 1000;

    private float _shapeSize = 0.2f;
    private float _half = 0.5f;
    private int _emitAmount = 30;

    private ObjectPool<ParticleSystem> StartTeleportPool;      
    private ObjectPool<ParticleSystem> TrailTeleportPool;      

    [Inject]
    private void Construct(ParticleSystem startEffect, ParticleSystem trailEffect)
    {
        _startTeleportEffect = startEffect;
        _trailTeleportEffect = trailEffect;

        InitializePools();
    }

    private void InitializePools()
    {
        StartTeleportPool = new ObjectPool<ParticleSystem>(
        createFunc: () => Instantiate(_startTeleportEffect),
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

    public void SpawnStartTeleportEffect(Transform spawnTransform)
    {
        ParticleSystem effect = StartTeleportPool.Get();

        effect.transform.SetParent(spawnTransform);
        
        effect.transform.localPosition = _startTeleportEffect.transform.position;
        effect.transform.localRotation = _startTeleportEffect.transform.rotation;
        
        effect.transform.SetParent(null);

        effect.Play();

        ReleaseOnLifetimeEnds(effect, StartTeleportPool).Forget();
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

    private async UniTask ReleaseOnLifetimeEnds(ParticleSystem obj, ObjectPool<ParticleSystem> pool)
    {
        await UniTask.Delay(_lifetime);

        if (obj != null)
        {
            pool.Release(obj);
        }
    }
}