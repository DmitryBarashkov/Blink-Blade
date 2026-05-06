using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class EndGameWindow : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private GameObject _gameObject;
    private float _duration = 1.5f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _gameObject = gameObject;
    }

    public void Setup(bool isOutOfEnergy = false)
    {
        _gameObject.SetActive(true);
        FadeIn(_duration);
    }

    public void Close() => _gameObject.SetActive(false);

    public class Factory : IFactory<AssetReference, IObservable<EndGameWindow>>
    {
        private readonly DiContainer _container;
        private readonly Transform _parent;

        public Factory(DiContainer container, [InjectOptional] Transform parent)
        {
            _container = container;
            _parent = parent;
        }

        public IObservable<EndGameWindow> Create(AssetReference assetRef)
        {
            var handle = assetRef.LoadAssetAsync<GameObject>();

            return handle.ToObservable().Select(prefab => _container.InstantiatePrefabForComponent<EndGameWindow>(handle.Result, _parent));
        }
    }

    private void FadeIn(float duration)
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, duration).SetUpdate(true);
    }
}
