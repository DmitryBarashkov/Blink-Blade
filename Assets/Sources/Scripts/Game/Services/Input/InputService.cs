using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InputService
{
    private const string Attack = "Fire1";
    private const string MenuOpen = "Cancel";
    private const string ChooseLevelMenu = "ChooseLevel";

    private CancellationTokenSource cts;
    private bool _isActive = false;
    private float _activateDelay = 0.1f;

    public event Action AttackBtnPressed;

    public event Action AttackBtnUp;

    public event Action MenuOpenBtnPressed;

    public event Action ChooseLevelBtnPressed;

    public void GetInput()
    {
        if (Input.GetButton(MenuOpen))
            MenuOpenBtnPressed?.Invoke();

        if (Input.GetButton(ChooseLevelMenu))
            ChooseLevelBtnPressed?.Invoke();

        if (_isActive == true)
        {
            if (Input.GetButton(Attack))
                AttackBtnPressed?.Invoke();
            if (Input.GetButtonUp(Attack))
                AttackBtnUp?.Invoke();
        }
    }

    public async void Activate()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();

        try
        {
            await UniTask.Delay(
                TimeSpan.FromSeconds(_activateDelay),
                delayType: DelayType.DeltaTime,
                cancellationToken: cts.Token);

            _isActive = true;
            Debug.Log("Ввод деактивирован через UniTask.");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Деактивация ввода была отменена.");
        }
    }

    public void Deactivate()
    {
        _isActive = false;
    }
}
