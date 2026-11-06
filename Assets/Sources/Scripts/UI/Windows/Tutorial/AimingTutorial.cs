using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BlinkBlade.UI
{
    public class AimingTutorial : MonoBehaviour
    {
        [Header("Настройка позиции элементов")]
        [SerializeField] private Vector3 _playerOffset = new Vector3(1.4f, 2.5f, 0f);
        [SerializeField] private Vector2 _handOffset = new Vector2(-1, -1);
        [SerializeField] private float _changeThreshold = 0.001f;

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
        private Vector2 _handInitialPosition;
        private Vector2 _arrowLastPosition;
        private Transform _target;

        public void Initialize(Transform target)
        {
            _target = target;
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void LateUpdate()
        {
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
            Vector2 handStartPos = _handInitialPosition;

            _tutorialSequence?.Kill();
            _tutorialSequence = DOTween.Sequence();

            _handTransform.anchoredPosition = _handInitialPosition;

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
            if (_target == null)
                return;

            Vector3 targetWorldPos = _target.position + _playerOffset;
            Vector3 screenPos = UnityEngine.Camera.main.WorldToScreenPoint(targetWorldPos);

            if (screenPos.z < 0)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _arrowTransform.parent as RectTransform,
                screenPos,
                null,
                out Vector2 arrowLocalPoint);

            _arrowTransform.anchoredPosition = arrowLocalPoint;

            if (IsArrowPositionHasChanged())
            {
                _handTransform.anchoredPosition = _handInitialPosition = _arrowTransform.anchoredPosition + _handOffset;
                _arrowLastPosition = _arrowTransform.anchoredPosition;

                PlayAimTutorial();
            }
        }

        private bool IsArrowPositionHasChanged()
        {
            float sqrDistance = (_arrowTransform.anchoredPosition - _arrowLastPosition).sqrMagnitude;

            return sqrDistance > _changeThreshold * _changeThreshold;
        }
    }
}
