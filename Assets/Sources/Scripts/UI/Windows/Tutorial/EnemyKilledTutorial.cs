using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BlinkBlade.UI
{
    public class EnemyKilledTutorial : InformationTutorial
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _crossImage;

        [Header("Настройки анимации")]
        [SerializeField] private float _colorEffectDuration = 0.5f;
        [SerializeField] private float _scaleFactor = 1.5f;
        [SerializeField] private float _scaleEffectDuration = 0.2f;
        [SerializeField] private float _shakeStrength = 10f;
        [SerializeField] private float _shakeEffectDuration = 0.3f;
        [SerializeField] private float _loopDelay = 1f;

        private Sequence _tutorialSequence;

        protected override void OnEnable()
        {
            base.OnEnable();

            PerformIconAnimation();
        }

        private void PerformIconAnimation()
        {
            _tutorialSequence = DOTween.Sequence();

            _crossImage.gameObject.SetActive(true);

            _tutorialSequence.Append(_iconImage.DOColor(Color.gray, _colorEffectDuration));

            Transform crossTransform = _crossImage.transform;
            Transform iconTransform = _iconImage.transform;

            crossTransform.localScale = Vector3.one * _scaleFactor;

            _tutorialSequence.Append(crossTransform.DOScale(Vector3.one, _scaleEffectDuration).SetEase(Ease.OutBack));
            _tutorialSequence.Append(iconTransform.DOShakePosition(_shakeEffectDuration, _shakeStrength));

            _tutorialSequence.AppendInterval(_loopDelay);
            _tutorialSequence.SetLoops(-1, LoopType.Restart);
        }
    }
}
