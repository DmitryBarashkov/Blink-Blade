using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinDatabase", menuName = "Config/Skins Database")]
public class SkinDatabase : ScriptableObject
{
    [Serializable]
    public struct PlayerSkin
    {
        public int id;
        public Player prefab;
        public Sprite preview;
        public int cost;
    }

    [Header("Player Skins")]
    public List<PlayerSkin> skins;

    public bool TryGetSkin(int id, out PlayerSkin result)
    {
        foreach (var skin in skins)
        {
            if (skin.id == id)
            {
                result = skin;
                return true;
            }
        }

        result = default;
        return false;
    }
}
