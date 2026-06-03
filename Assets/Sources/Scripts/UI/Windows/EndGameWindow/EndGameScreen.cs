using System;
using UniRx;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class EndGameScreen : UIScreen, IPointerClickHandler
{
    private CanvasGroup _canvasGroup;
    private GameObject _gameObject;
    private float _duration = 1.5f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _gameObject = gameObject;
    }

    public override void Setup()
    {
        _gameObject.SetActive(true);
        _canvasGroup.interactable = false;
        FadeIn(_duration);
    }

    public void Close() => _gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData) => _canvasGroup.DOComplete();

    private void FadeIn(float duration)
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, duration).SetUpdate(true)
            .OnComplete(() => _canvasGroup.interactable = true);
    }
}
