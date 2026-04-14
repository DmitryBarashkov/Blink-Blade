using UnityEngine;

public static class Utils
{
    public static void FixPositionZ(Transform transform, float fixedZ = 0)
    {
        Vector3 fixedZPosition = transform.position;
            
        fixedZPosition.z = fixedZ;
        transform.position = fixedZPosition;
    }
}
