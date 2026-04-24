using System;
using UnityEngine;

public class Thrower
{
    private Weapon _weapon;

    public Thrower(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void Throw(Vector3 direction)
    {
        if (_weapon == null)
            throw new ArgumentNullException(nameof(_weapon));

        if (direction == Vector3.zero)
            throw new ArgumentNullException(nameof(direction));

        if (_weapon != null)
        {
            _weapon.Throw(direction);
        }
    }    
}
