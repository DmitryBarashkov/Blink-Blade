using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BlinkBlade.Game
{
    public class InputService
    {
        private const string Attack = "Fire1";
        private const string MenuOpen = "Cancel";
        private const string ChooseLevelMenu = "ChooseLevel";

        private CancellationTokenSource _token;
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
            _token?.Cancel();
            _token?.Dispose();
            _token = new CancellationTokenSource();

            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(_activateDelay),
                    delayType: DelayType.DeltaTime,
                    cancellationToken: _token.Token);

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
}