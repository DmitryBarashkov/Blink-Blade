using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BlinkBlade.Game
{
    public class ObjectPoolService : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<GameObject>> Pools = new ();
        private readonly Dictionary<PoolObjectTypes, PoolType> TypeToConfigMap = new ();

        [SerializeField] private List<PoolType> _poolConfig;

        private Transform _poolContainer;
        private DiContainer _container;

        public enum PoolObjectTypes
        {
            TeleportEffect,

            HitEffectBlood,
            HitEffectMetal,
            HitEffectGrass,
            HitEffectWood,
            HitEffectStone,

            Arrow,
        }

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

        public GameObject Get(PoolObjectTypes objectType, Vector3 position, Quaternion rotation)
        {
            if (!TypeToConfigMap.TryGetValue(objectType, out PoolType poolType))
                return null;

            int id = poolType.Id;
            float lifeTime = poolType.LifeTime;

            if (!Pools.ContainsKey(id))
                return null;

            GameObject obj = Pools[id].Count > 0 ? Pools[id].Dequeue() : CreateNewObject(poolType);

            if (obj == null)
            {
                obj = CreateNewObject(poolType);
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            if (lifeTime > 0)
                ReleaseAfterLifeTimeEnds(id, obj, lifeTime, this.GetCancellationTokenOnDestroy()).Forget();

            return obj;
        }

        public void Release(int id, GameObject obj)
        {
            if (obj == null)
                return;

            obj.SetActive(false);

            if (Pools[id].Contains(obj) == false)
                Pools[id].Enqueue(obj);
        }

        public void Release(PoolObjectTypes type, GameObject obj)
        {
            if (obj == null)
                return;

            if (!TypeToConfigMap.TryGetValue(type, out PoolType poolType))
            {
                Debug.LogError($"[ObjectPoolService] Конфигурация для типа {type} не найдена!");
                return;
            }

            int id = poolType.Id;

            obj.SetActive(false);

            if (Pools[id].Contains(obj) == false)
                Pools[id].Enqueue(obj);
        }

        private void InitPools()
        {
            foreach (var config in _poolConfig)
            {
                Pools[config.Id] = new Queue<GameObject>();
                TypeToConfigMap[config.ObjectType] = config;

                for (int i = 0; i < config.PoolSize; i++)
                {
                    CreateNewObject(config);
                }
            }
        }

        private GameObject CreateNewObject(PoolType poolType)
        {
            GameObject obj = _container.InstantiatePrefab(poolType.Prefab);

            obj.SetActive(false);
            obj.transform.SetParent(_poolContainer);

            Pools[poolType.Id].Enqueue(obj);

            return obj;
        }

        private async UniTaskVoid ReleaseAfterLifeTimeEnds(int id, GameObject obj, float lifeTime, CancellationToken token)
        {
            bool isCanceled = await UniTask.Delay(
                TimeSpan.FromSeconds(lifeTime),
                delayTiming: PlayerLoopTiming.Update,
                cancellationToken: token).SuppressCancellationThrow();

            if (isCanceled || obj == null || obj.activeSelf == false)
                return;

            Release(id, obj);
        }

        [Serializable]
        public struct PoolType
        {
            public int Id;
            public PoolObjectTypes ObjectType;
            public GameObject Prefab;
            public int PoolSize;
            public float LifeTime;
        }
    }
}