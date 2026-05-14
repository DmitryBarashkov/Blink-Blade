using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class AimingArrow : MonoBehaviour
{
    [SerializeField] private float _offset = 50f;
    [SerializeField] private float _playerHeightOffset = 1.2f;

    private RectTransform _rectTransform;
    private Vector2 _direction;
    private float _maxWidth;

    public Vector2 Direction => _direction;

    [Inject]
    private void Construct()
    {
        _rectTransform = GetComponent<RectTransform>();
        _maxWidth = _rectTransform.sizeDelta.x;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _rectTransform.sizeDelta = new Vector2(_maxWidth, _rectTransform.sizeDelta.y);

        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 playerPosition)
    {
        if (playerPosition == Vector3.zero)
            throw new ArgumentNullException(nameof(playerPosition));

        Vector3 position = playerPosition + _playerHeightOffset * Vector3.up;
        Vector2 playerCanvasPosition = Camera.main.WorldToScreenPoint(position);
        Vector2 targetPosition = Input.mousePosition;
        
        _direction = (targetPosition - playerCanvasPosition).normalized;
        
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        float scaledOffset = _offset * _rectTransform.lossyScale.x;

        _rectTransform.position = playerCanvasPosition + _direction * scaledOffset;
        _rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        float distance = (targetPosition - playerCanvasPosition).magnitude;

        if (distance > 0.01f)
        {
            float width = Mathf.Clamp(distance, 0, _maxWidth);

            _rectTransform.sizeDelta = new Vector2(width, _rectTransform.sizeDelta.y);
        }
    }
}
