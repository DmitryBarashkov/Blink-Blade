using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputService
{
    private const string Attack = "Fire1";
    private const string MenuOpen = "Cancel";
    private const string ChooseLevelMenu = "ChooseLevel";

    public event Action AttackBtnPressed;
    public event Action AttackBtnUp;
    public event Action MenuOpenBtnPressed;
    public event Action ChooseLevelBtnPressed;

    private bool _isActive = false;

    public void GetInput()
    {
        if (Input.touchCount > 0)
            HandleMobileTouch();
        else
            HandlePCInput();        
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    private void HandlePCInput()
    {
        if (Input.GetButton(MenuOpen))
        {
            MenuOpenBtnPressed?.Invoke();
        }
        if (Input.GetButton(ChooseLevelMenu))
        {
            ChooseLevelBtnPressed?.Invoke();
        }

        if (_isActive == true)
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

    private void HandleMobileTouch()
    {
        if (_isActive == true)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                if (Input.GetButton(Attack))
                    AttackBtnPressed?.Invoke();
                
                if (Input.GetButtonUp(Attack))
                    AttackBtnUp?.Invoke();
            }
        }
    }
}
