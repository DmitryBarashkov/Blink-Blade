using UnityEngine;

public class WeaponRotator
{
    private Transform _weaponTransform;
    private float _stickOffsetAngle;

    private Vector3 _bladeDirection = Vector3.up;

    public WeaponRotator(Transform weaponTransform, float stickOffsetAngle)
    {
        _weaponTransform = weaponTransform;
        _stickOffsetAngle = stickOffsetAngle;
    }

    public void RotateToObstacle(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Vector3 surfaceNormal = contact.normal;
        Vector3 targetBladeDir = -surfaceNormal;

        targetBladeDir.z = 0f;
        targetBladeDir.Normalize();

        Quaternion targetRotation = Quaternion.FromToRotation(_bladeDirection, targetBladeDir);
        Vector3 euler = targetRotation.eulerAngles;

        targetRotation = Quaternion.Euler(0f, 0f, euler.z);

        if (targetBladeDir.x > 0)
        {
            targetRotation *= Quaternion.Euler(0, 180f, -_stickOffsetAngle);
        }
        else
        {
            targetRotation *= Quaternion.Euler(0, 0, -_stickOffsetAngle);
        }

        _weaponTransform.rotation = targetRotation;
    }

    public void ResetRotation(float rotationAngle)
    {
        if (rotationAngle == 0)
            return;

        if (rotationAngle > 0)
        {
            _weaponTransform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            _weaponTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void RotateBladeForward(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _weaponTransform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
}
