using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void FixPositionZ(Transform transform, float fixedZ = 0)
    {
        Vector3 fixedZPosition = transform.position;
            
        fixedZPosition.z = fixedZ;
        transform.position = fixedZPosition;
    }

    public static T GetRandomElement<T>(IReadOnlyList<T> list)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentNullException(nameof(list));

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
