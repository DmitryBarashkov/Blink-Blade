using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinDatabase", menuName = "Config/Skins Database")]
public class SkinDatabase : ScriptableObject
{
    [Header("Player Skins")]
    public List<PlayerSkin> Skins;

    [Serializable]
    public struct PlayerSkin
    {
        public int Id;
        public Player Prefab;
        public Sprite Preview;
        public int Cost;
    }

    public bool TryGetSkin(int id, out PlayerSkin result)
    {
        foreach (var skin in Skins)
        {
            if (skin.Id == id)
            {
                result = skin;
                return true;
            }
        }

        result = default;
        return false;
    }
}
