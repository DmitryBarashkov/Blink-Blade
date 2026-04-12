using System;
using UnityEngine;

public class InputService: MonoBehaviour
{
    private const string Attack = "Fire1";

    public event Action AttackBtnPressed;
    public event Action AttackBtnUp;

    public void GetInput()
    {
        if (Input.GetButton(Attack))
        {
            AttackBtnPressed?.Invoke();
        }
        if (Input.GetButtonUp(Attack))
        {
            AttackBtnUp?.Invoke();
        }
    }
}
