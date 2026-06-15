using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Config/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    [Serializable]
    public struct PlayerWeapon
    {
        public int id;
        public Weapon prefab;
        public Sprite preview;
    }

    [Header("Player Weapons")]
    public List<PlayerWeapon> weapons;

    public bool TryGetWeapon(int id, out PlayerWeapon result)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.id == id)
            {
                result = weapon;
                return true;
            }
        }

        result = default;
        return false;
    }
}
