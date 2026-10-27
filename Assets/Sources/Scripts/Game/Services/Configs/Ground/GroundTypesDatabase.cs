using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BlinkBlade.Game
{
    [CreateAssetMenu(fileName = "GroundTypeDatabase", menuName = "Config/Ground Type Database")]
    public class GroundTypesDatabase : ScriptableObject
    {
        [Header("GroundType")]
        [FormerlySerializedAs("GroundTypes")]
        [SerializeField] private List<GroundTypeRecord> _groundTypes;

        public bool TryGetGroundType(GroundType type, out GroundTypeRecord result)
        {
            foreach (var groundType in _groundTypes)
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

        [Serializable]
        public struct GroundTypeRecord
        {
            public GroundType Type;
            public float BounceForce;
        }
    }
}