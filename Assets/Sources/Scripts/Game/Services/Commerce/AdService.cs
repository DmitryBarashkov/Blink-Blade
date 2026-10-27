using System;
using System.Threading.Tasks;
using YG;
using Zenject;

namespace BlinkBlade.Game
{
    public class AdService : IDisposable
    {
        private IAudioService _audioService;
        private Action _currentActiveCallback;
        private int _adTimeoutCheck = 1000;
        private bool _isAdResponseReceived = false;

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;

            Initialize();
        }

        public void Dispose()
        {
            YG2.onErrorRewardedAdv -= () => HandleRewardedError("YG_EventError");
        }

        public void ShowAdvForReward(string rewardId, Action callback)
        {
            if (_currentActiveCallback != null)
                return;

            _audioService.PlaySound(SoundType.ButtonClick);
            _audioService.Deactivate();

            _currentActiveCallback = callback;
            _isAdResponseReceived = false;

            StartTimeoutCheck();

            YG2.RewardedAdvShow(rewardId, () =>
            {
                _isAdResponseReceived = true;
                _audioService.Activate();
                callback?.Invoke();
                _currentActiveCallback = null;
            });
        }

        private void Initialize()
        {
            YG2.onErrorRewardedAdv += () => HandleRewardedError("YG_EventError");
        }

        private async void HandleRewardedError(string reason)
        {
            UnityEngine.Debug.LogWarning($"HandleRewardError - callback:{_currentActiveCallback != null} Platform:{YG2.platform}");

            if (_currentActiveCallback != null && YG2.platform == "CrazyGames")
            {
                _audioService.Activate();
                _currentActiveCallback?.Invoke();
                _currentActiveCallback = null;
            }
        }

        private async void StartTimeoutCheck()
        {
            await Task.Delay(_adTimeoutCheck);

            if (_isAdResponseReceived == false && _currentActiveCallback != null)
                HandleRewardedError("Timeout_BasicLaunch");
        }
    }
}
