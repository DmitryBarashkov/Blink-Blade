using UnityEngine;

public class AimingArrow : MonoBehaviour
{
    private float _sensitivity = 1f;
    private float _maxScale = 1.5f;

    public void SetPosition(Vector3 target)
    {
        Vector2 direction = target - transform.position;

        SetRotation(direction);
        SetScale(direction);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SetScale(Vector2 direction)
    {
        float distance = direction.magnitude * _sensitivity;
        float currentScale = Mathf.Min(distance, _maxScale);

        transform.localScale = new Vector3(currentScale, 1, 1);
    }
}
