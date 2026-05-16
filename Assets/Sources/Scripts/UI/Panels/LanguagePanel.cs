using DG.Tweening;
using UnityEngine;

public class LanguagePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private RectTransform _rectTransform;

    private bool _isOpen = false;

    private float _expandedWidth = 330f;
    private float _collapsedWidth = 0;

    private float _duration = 0.3f;
    private Ease _easeType = Ease.InOutQuad;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(_collapsedWidth, _rectTransform.sizeDelta.y);

        _canvasGroup.alpha = 0f;
        SetInteraction(false);
    }

    public void ToggleMenu()
    {
        _rectTransform.DOKill();
        _canvasGroup.DOKill();

        if (!_isOpen)
        {
            _rectTransform.DOSizeDelta(new Vector2(_expandedWidth, _rectTransform.sizeDelta.y), _duration).SetEase(_easeType);
            _canvasGroup.DOFade(1f, _duration);

            SetInteraction(true);
            _isOpen = true;
        }
        else
        {
            _rectTransform.DOSizeDelta(new Vector2(_collapsedWidth, _rectTransform.sizeDelta.y), _duration).SetEase(_easeType);
            _canvasGroup.DOFade(0f, _duration);

            SetInteraction(false);
            _isOpen = false;
        }
    }

    private void SetInteraction(bool state)
    {
        _canvasGroup.interactable = state;
        _canvasGroup.blocksRaycasts = state;
    }
}
