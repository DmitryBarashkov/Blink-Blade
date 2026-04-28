using System;
using UnityEngine;
using Zenject;

public class AimingArrow : MonoBehaviour
{
    private RectTransform _rectTransform;

    private float _offset = 40f;

    [Inject]
    private void Construct()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Transform weaponHandler, Transform target)
    {
        Vector2 weaponHandlerCanvasPosition = Camera.main.WorldToScreenPoint(weaponHandler.position);
        Vector2 targetCanvasPosition = Camera.main.WorldToScreenPoint(target.position);
        Vector2 direction = targetCanvasPosition - weaponHandlerCanvasPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _rectTransform.position = weaponHandlerCanvasPosition + direction.normalized * _offset;
        _rectTransform.rotation = Quaternion.Euler(0, 0, angle - 180f);
    }
}
