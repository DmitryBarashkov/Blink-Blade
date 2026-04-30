using UnityEngine;

public class LoadingIndicator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _pulseSpeed = 200f;
    [SerializeField] private float _pulseAmount = 200f;

    private Transform _transform;
    private Vector3 _initialScale;

    private void Awake()
    {
        _transform = transform;
        _initialScale = _transform.localScale;
    }

    private void Update()
    {
        float scaleOffset = Mathf.Sin(Time.time * _pulseSpeed) * _pulseAmount;

        _transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
        _transform.localScale = _initialScale + new Vector3(scaleOffset, scaleOffset, scaleOffset);
    }
}
