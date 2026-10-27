using BlinkBlade.Game;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BlinkBlade.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class InformationTutorial : MonoBehaviour
    {
        [Inject] private InputService _input;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _textTransform;

        private Button _button;
        private GameObject _gameObject;

        private float _duration = 0.15f;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _gameObject = gameObject;
        }

        protected virtual void OnEnable()
        {
            _input.Deactivate();
            _button.onClick.AddListener(CloseTutorial);
            _canvasGroup.blocksRaycasts = true;

            PerformTextAnimation();
        }

        private void OnDisable()
        {
            _input.Activate();
            _button.onClick.RemoveListener(CloseTutorial);
            _canvasGroup.blocksRaycasts = false;
        }

        private void PerformTextAnimation()
        {
            _textTransform.DOScale(0f, 0);

            _textTransform.DOScale(1f, _duration).SetEase(Ease.InQuad);
        }

        private void CloseTutorial()
        {
            _gameObject.SetActive(false);
        }
    }
}
