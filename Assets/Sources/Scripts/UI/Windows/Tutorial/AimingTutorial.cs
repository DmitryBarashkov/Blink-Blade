using System;
using BlinkBlade.Game;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BlinkBlade.UI
{
    public class AimingTutorial : MonoBehaviour
    {
        [Header("UI Элементы")]
        [SerializeField] private RectTransform _handTransform;
        [SerializeField] private RectTransform _arrowTransform;
        [SerializeField] private Image _arrowImage;
        [SerializeField] private Image _handImage;

        [Header("Настройки позиций")]
        [SerializeField] private Vector3 _clickScale = new Vector3(0.85f, 0.85f, 0.85f);
        [SerializeField] private float _handTargetX = 10f;
        [SerializeField] private float _handTargetY = 10f;

        [Header("Тайминги")]
        [SerializeField] private float _clickDuration = 0.3f;
        [SerializeField] private float _animationDuration = 0.8f;
        [SerializeField] private float _reverseAnimationDuration = 0.15f;
        [SerializeField] private float _loopDelay = 1f;

        private Sequence _tutorialSequence;
        private Vector2 _initialPosition;

        private float _albumArrowYPosition = 630f;
        private float _albumHandYPosition = 600f;
        private float _portraitArrowYPosition = 570f;
        private float _portraitHandYPosition = 540f;

        private int _lastWidth;
        private int _lastHeight;
        private ScreenOrientation _lastOrientation;

        private void Awake()
        {
            _initialPosition = _handTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            PlayAimTutorial();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void Update()
        {
            if (Screen.width != _lastWidth || Screen.height != _lastHeight || Screen.orientation != _lastOrientation)
                ApplyNewOffsets();
        }

        private void OnDestroy()
        {
            StopAnimation();
        }

        private void StopAnimation()
        {
            _tutorialSequence?.Kill();
        }

        private void PlayAimTutorial()
        {
            Vector2 handStartPos = _initialPosition;

            _tutorialSequence?.Kill();
            _tutorialSequence = DOTween.Sequence();

            _handTransform.anchoredPosition = _initialPosition;

            _handTransform.localScale = Vector3.one;
            _arrowTransform.localScale = Vector3.zero;

            _arrowImage.DOFade(0, 0);
            _handImage.DOFade(1f, 0);

            _tutorialSequence.Append(_handTransform.DOScale(_clickScale, _clickDuration).SetEase(Ease.InQuad));
            _tutorialSequence.Append(_arrowImage.DOFade(1f, 0.2f));

            float targetXHand = handStartPos.x + _handTargetX;
            float targetYHand = handStartPos.y + _handTargetY;

            _tutorialSequence.Join(_arrowTransform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutCubic));
            _tutorialSequence.Join(_handTransform.DOAnchorPos(new Vector2(targetXHand, targetYHand), _animationDuration).SetEase(Ease.OutCubic));

            _tutorialSequence.Append(_handTransform.DOScale(Vector3.one, _clickDuration).SetEase(Ease.OutQuad));
            _tutorialSequence.Join(_arrowTransform.DOScale(Vector3.zero, _reverseAnimationDuration).SetEase(Ease.OutQuad));
            _tutorialSequence.Join(_arrowImage.DOFade(0, _reverseAnimationDuration).SetEase(Ease.OutQuad));
            _tutorialSequence.Join(_handImage.DOFade(0, _reverseAnimationDuration).SetEase(Ease.OutQuad));

            _tutorialSequence.AppendInterval(_loopDelay);

            _tutorialSequence.SetLoops(-1, LoopType.Restart);
        }

        private void ApplyNewOffsets()
        {
            bool isPortrait = Screen.width < Screen.height;

            ResetTrackedValues();

            _handTransform.anchoredPosition = _initialPosition;

            if (isPortrait)
            {
                _arrowTransform.anchoredPosition = new Vector2(_arrowTransform.anchoredPosition.x, _portraitArrowYPosition);
                _handTransform.anchoredPosition = new Vector2(_handTransform.anchoredPosition.x, _portraitHandYPosition);
            }
            else
            {
                _arrowTransform.anchoredPosition = new Vector2(_arrowTransform.anchoredPosition.x, _albumArrowYPosition);
                _handTransform.anchoredPosition = new Vector2(_handTransform.anchoredPosition.x, _albumHandYPosition);
            }
        }

        private void ResetTrackedValues()
        {
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
            _lastOrientation = Screen.orientation;
        }
    }
}
