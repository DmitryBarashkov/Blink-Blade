
using UnityEngine;

public class WeaponRotator
{
    private Transform _weaponTransform;
    private float _rotationOffsetAngle;

    private Vector3 _bladeDirection = Vector3.up;

    public WeaponRotator(Transform weaponTransform, float rotationOffsetAngle)
    {
        _weaponTransform = weaponTransform;
        _rotationOffsetAngle = rotationOffsetAngle;        
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
            targetRotation *= Quaternion.Euler(0, 180f, -_rotationOffsetAngle);
        }
        else
        {
            targetRotation *= Quaternion.Euler(0, 0, -_rotationOffsetAngle);
        }

        _weaponTransform.rotation = targetRotation;
    }

    public void ResetRotation(float rotationAngle)
    {
        if (rotationAngle == 0)
            return;

        if (rotationAngle > 0)
        {
            _weaponTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _weaponTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
