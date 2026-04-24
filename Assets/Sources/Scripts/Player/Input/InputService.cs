using System;
using UnityEngine;

public class InputService
{
    private const string Attack = "Fire1";

    public event Action AttackBtnPressed;
    public event Action AttackBtnUp;

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
