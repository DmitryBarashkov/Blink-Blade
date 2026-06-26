using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ObjectPoolService : MonoBehaviour
{
    public enum PoolObjectTypes
    {
        TeleportEffect,

        HitEffectBlood,
        HitEffectMetal,
        HitEffectGrass,
        HitEffectWood,
        HitEffectStone,

        Arrow
    }

    [Serializable]
    public struct PoolType
    {
        public int id;
        public PoolObjectTypes objectType;
        public GameObject prefab;
        public int poolSize;
        public float lifeTime;
    }

    [SerializeField] private List<PoolType> _poolConfig;

    private readonly Dictionary<int, Queue<GameObject>> _pools = new();
    private readonly Dictionary<PoolObjectTypes, PoolType> _typeToConfigMap = new();
    private Transform _poolContainer;
    private DiContainer _container;

    [Inject]
    public void Construct(Transform poolContainer, DiContainer container)
    {
        _poolContainer = poolContainer;
        _container = container;
    }

    private void Awake()
    {
        InitPools();
    }

    private void InitPools()
    {
        foreach (var config in _poolConfig)
        {
            _pools[config.id] = new Queue<GameObject>();
            _typeToConfigMap[config.objectType] = config;

            for (int i = 0; i < config.poolSize; i++)
            {
                CreateNewObject(config);
            }
        }
    }

    private GameObject CreateNewObject(PoolType poolType)
    {
        GameObject obj = _container.InstantiatePrefab(poolType.prefab);
        
        obj.SetActive(false);
        obj.transform.SetParent(_poolContainer);

        _pools[poolType.id].Enqueue(obj);
        
        return obj;
    }

    public GameObject Get(PoolObjectTypes objectType, Vector3 position, Quaternion rotation)
    {
        if (!_typeToConfigMap.TryGetValue(objectType, out PoolType poolType))
            return null;

        int id = poolType.id;
        float lifeTime = poolType.lifeTime;

        if (!_pools.ContainsKey(id)) 
            return null;

        GameObject obj = _pools[id].Count > 0 ? _pools[id].Dequeue() : CreateNewObject(poolType);

        if (obj == null)
        {
            obj = CreateNewObject(poolType);
        }

        obj.transform.SetPositionAndRotation(position, rotation);        
        obj.SetActive(true);
        
        if (lifeTime > 0)
        {
            ReleaseAfterLifeTimeEnds(id, obj, lifeTime, this.GetCancellationTokenOnDestroy()).Forget();
        }

        return obj;
    }

    private async UniTaskVoid ReleaseAfterLifeTimeEnds(int id, GameObject obj, float lifeTime, CancellationToken token)
    {
        bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(lifeTime), 
                                              delayTiming: PlayerLoopTiming.Update, 
                                              cancellationToken: token).SuppressCancellationThrow();

        if (isCanceled || obj == null || obj.activeSelf == false)
            return;

        Release(id, obj);
    }

    public void Release(int id, GameObject obj)
    {
        if (obj == null)
            return;
        
        obj.SetActive(false);

        if (_pools[id].Contains(obj) == false)
        {
            _pools[id].Enqueue(obj);
        }
    }

    public void Release(PoolObjectTypes type, GameObject obj)
    {
        if (obj == null)
            return;

        if (!_typeToConfigMap.TryGetValue(type, out PoolType poolType))
        {
            Debug.LogError($"[ObjectPoolService] Конфигурация для типа {type} не найдена!");
            return;
        }

        int id = poolType.id;
        
        obj.SetActive(false);


        if (_pools[id].Contains(obj) == false)
        {
            _pools[id].Enqueue(obj);
        }
    }
}
