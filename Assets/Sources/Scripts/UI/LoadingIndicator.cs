using DG.Tweening;
using UnityEngine;

public class LoadingIndicator : MonoBehaviour
{
    [SerializeField] private float _rotationDuration = 2f;
    [SerializeField] private float _pulseDuration = 1f;
    [SerializeField] private float _pulseAmount = 0.2f;

    private Transform _transform;    

    private void Awake()
    {
        _transform = transform;        
    }

    private void Start()
    {
        _transform.DORotate(new Vector3(0, 0, 360), _rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);

        _transform.DOScale(transform.localScale + Vector3.one * _pulseAmount, _pulseDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnDestroy()
    {
        _transform.DOKill();
    }
}
