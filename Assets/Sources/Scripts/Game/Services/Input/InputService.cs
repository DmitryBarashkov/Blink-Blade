using System;
using UnityEngine;

public class InputService
{
    private const string Attack = "Fire1";
    private const string MenuOpen = "Cancel";

    public event Action AttackBtnPressed;
    public event Action AttackBtnUp;
    public event Action MenuOpenBtnPressed;

    private bool _isActive = false;

    public void GetInput()
    {
        if (_isActive == false)
            return;
        
        if (Input.GetButton(Attack))
        {
            AttackBtnPressed?.Invoke();
        }
        if (Input.GetButtonUp(Attack))
        {
            AttackBtnUp?.Invoke();
        }
        if (Input.GetButton(MenuOpen))
        {
            MenuOpenBtnPressed?.Invoke();
        }

    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }
}
