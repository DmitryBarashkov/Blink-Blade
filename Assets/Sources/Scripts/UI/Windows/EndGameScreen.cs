using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class EndGameScreen : MonoBehaviour, IPointerClickHandler
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
        _canvasGroup.interactable = false;
        FadeIn(_duration);
    }

    public void Close() => _gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData) => _canvasGroup.DOComplete();

    public class Factory : IFactory<AssetReference, IObservable<EndGameScreen>>
    {
        private readonly DiContainer _container;
        private readonly Transform _parent;

        public Factory(DiContainer container, [InjectOptional] Transform parent)
        {
            _container = container;
            _parent = parent;
        }

        public IObservable<EndGameScreen> Create(AssetReference assetRef)
        {
            var handle = assetRef.LoadAssetAsync<GameObject>();

            return handle.ToObservable().Select(prefab => _container.InstantiatePrefabForComponent<EndGameScreen>(handle.Result, _parent));
        }
    }

    private void FadeIn(float duration)
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, duration).SetUpdate(true)
            .OnComplete(() => _canvasGroup.interactable = true);
    }
}
