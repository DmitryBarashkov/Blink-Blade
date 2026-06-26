using System;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType
{
    Grass,
    Wood,
    Stone
}

[CreateAssetMenu(fileName = "GroundTypeDatabase", menuName = "Config/Ground Type Database")]
public class GroundTypeDatabase : ScriptableObject
{
    [Serializable]
    public struct GroundTypeRecord
    {
        public GroundType type;
        public float bounceForce;
    }

    [Header("GroundType")]
    public List<GroundTypeRecord> groundTypes;

    public bool TryGetGroundType(GroundType type, out GroundTypeRecord result)
    {
        foreach (var groundType in groundTypes)
        {
            if (type == groundType.type)
            {
                result = groundType;
                return true;
            }
        }

        result = default;
        return false;
    }
}
