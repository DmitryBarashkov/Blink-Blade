using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyIcon: MonoBehaviour
{
    private Image _iconImage;
    private Image _crossImage;

    private float _colorEffectDuration = 0.5f;
    private float _scaleFactor = 1.5f;
    private float _scaleEffectDuration = 0.2f;
    private float _shakeStrength = 10f;
    private float _shakeEffectDuration = 0.3f;

    private void Awake()
    {
        _iconImage = GetComponent<Image>();
        _crossImage = GetComponentInChildren<CrossImage>(true).GetComponent<Image>();
    }

    public void MarkAsDead()
    {
        _crossImage.gameObject.SetActive(true);

        _iconImage.DOColor(Color.gray, _colorEffectDuration);

        _crossImage.transform.localScale = Vector3.one * _scaleFactor;
        _crossImage.transform.DOScale(Vector3.one, _scaleEffectDuration).SetEase(Ease.OutBack);

        _iconImage.transform.DOShakePosition(_shakeEffectDuration, _shakeStrength);        
    }
}
