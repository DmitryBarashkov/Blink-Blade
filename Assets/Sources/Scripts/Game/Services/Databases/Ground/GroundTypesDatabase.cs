using System;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType
{
    Grass,
    Wood,
    Stone,
}

[CreateAssetMenu(fileName = "GroundTypeDatabase", menuName = "Config/Ground Type Database")]
public class GroundTypesDatabase : ScriptableObject
{
    [Header("GroundType")]
    public List<GroundTypeRecord> GroundTypes;

    [Serializable]
    public struct GroundTypeRecord
    {
        public GroundType Type;
        public float BounceForce;
    }

    public bool TryGetGroundType(GroundType type, out GroundTypeRecord result)
    {
        foreach (var groundType in GroundTypes)
        {
            if (type == groundType.Type)
            {
                result = groundType;
                return true;
            }
        }

        result = default;
        return false;
    }
}
