using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Config/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    [Header("Player Weapons")]
    public List<PlayerWeapon> Weapons;

    [Serializable]
    public struct PlayerWeapon
    {
        public int Id;
        public Weapon Prefab;
        public Sprite Preview;
    }

    public bool TryGetWeapon(int id, out PlayerWeapon result)
    {
        foreach (var weapon in Weapons)
        {
            if (weapon.Id == id)
            {
                result = weapon;
                return true;
            }
        }

        result = default;
        return false;
    }
}
