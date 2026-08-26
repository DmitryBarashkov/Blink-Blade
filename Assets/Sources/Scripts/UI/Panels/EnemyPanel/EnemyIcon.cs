using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : MonoBehaviour
{
    [SerializeField] private Image _crossImage;

    private Image _iconImage;
    private Transform _iconTransform;

    private GameObject _crossGameObject;
    private Transform _crossTransform;

    private float _colorEffectDuration = 0.5f;
    private float _scaleFactor = 1.5f;
    private float _scaleEffectDuration = 0.2f;
    private float _shakeStrength = 10f;
    private float _shakeEffectDuration = 0.3f;

    public bool IsMarked { get; private set; }

    private void Awake()
    {
        _iconImage = GetComponent<Image>();

        _iconTransform = _iconImage.transform;
        _crossGameObject = _crossImage.gameObject;
        _crossTransform = _crossImage.transform;
    }

    public void Reset()
    {
        IsMarked = false;

        _crossGameObject.SetActive(false);

        _iconImage.color = Color.white;

        _crossTransform.localScale = Vector3.one;
    }

    public void MarkAsDead()
    {
        IsMarked = true;

        _crossGameObject.SetActive(true);

        _iconImage.DOColor(Color.gray, _colorEffectDuration);

        _crossTransform.localScale = Vector3.one * _scaleFactor;
        _crossTransform.DOScale(Vector3.one, _scaleEffectDuration).SetEase(Ease.OutBack);

        _iconTransform.DOShakePosition(_shakeEffectDuration, _shakeStrength);
    }
}
