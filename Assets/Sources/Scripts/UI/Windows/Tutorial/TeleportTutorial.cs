using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlinkBlade.UI
{
    public class TeleportTutorial : MonoBehaviour
    {
        [SerializeField] private FirstTutorialScreen _screen;
        [SerializeField] private Transform _handTransform;
        [SerializeField] private Image _handImage;
        [SerializeField] private Transform _textTransform;

        [SerializeField] private Vector3 _clickScale = new Vector3(0.85f, 0.85f, 0.85f);

        [Header("Тайминги")]
        [SerializeField] private float _clickDuration = 0.3f;
        [SerializeField] private float _animationDuration = 0.8f;
        [SerializeField] private float _reverseAnimationDuration = 0.15f;
        [SerializeField] private float _loopDelay = 1f;
        [SerializeField] private float _showDelay = 3f;

        private Sequence _tutorialSequence;
        private float _showTimer = 0f;
        private bool _isActive;

        private void OnEnable()
        {
            _handTransform.gameObject.SetActive(false);
            _textTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isActive == true)
                return;

            if (_showTimer <= _showDelay)
            {
                _showTimer += Time.deltaTime;
            }
            else
            {
                if (_screen.HasActiveTutorial())
                    _showTimer = 0f;
                else
                    PlayTutorial();
            }
        }

        private void PlayTutorial()
        {
            _isActive = true;

            _handTransform.gameObject.SetActive(true);
            _textTransform.gameObject.SetActive(true);

            _tutorialSequence?.Kill();
            _tutorialSequence = DOTween.Sequence();

            _textTransform.DOScale(0f, 0);
            _handImage.DOFade(1f, 0);

            _textTransform.DOScale(1f, _reverseAnimationDuration).SetEase(Ease.InQuad);

            _tutorialSequence.Append(_handTransform.DOScale(_clickScale, _clickDuration).SetEase(Ease.OutQuad));
            _tutorialSequence.Append(_handTransform.DOScale(Vector3.one, _reverseAnimationDuration).SetEase(Ease.OutQuad));

            _tutorialSequence.AppendInterval(_loopDelay);

            _tutorialSequence.SetLoops(-1, LoopType.Restart);
        }
    }
}
