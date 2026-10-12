using System;
using BlinkBlade.Common;
using BlinkBlade.Game;
using BlinkBlade.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace BlinkBlade.UI
{
    public class ShopEnergyButton : UIButton
    {
        [Inject] private PlayerStats _playerStats;
        [Inject] private AdService _adService;

        [Header("UI Elements")]
        [SerializeField] private Button _rewardedButton;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private Image _icon;

        [Header("Settings")]
        [SerializeField] private float _cooldownDuration = 60f;

        private string _rewardId = "AddPlayerEnergy";
        private float _currentCooldownTime = 0f;
        private bool _isCooldown = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            YG2.onGetSDKData += CheckSavedCooldown;

            _cooldownText.gameObject.SetActive(false);

            if (YG2.isSDKEnabled)
                CheckSavedCooldown();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            YG2.onGetSDKData -= CheckSavedCooldown;
        }

        private void Update()
        {
            if (_isCooldown)
            {
                _currentCooldownTime -= Time.deltaTime;

                if (_currentCooldownTime <= 0)
                    EndCooldown();
                else
                    _cooldownText.text = $"{Mathf.CeilToInt(_currentCooldownTime)}";
            }
        }

        public override void HandleClick()
        {
            if (_isCooldown)
                return;

            _adService.ShowAdvForReward(_rewardId, GetAward);
        }

        private void GetAward()
        {
            YG2.saves.Energy++;
            YG2.saves.RewardedAdvCooldownEndTime = (YG2.ServerTime() / 1000) + (long)_cooldownDuration;
            YG2.SaveProgress();

            _playerStats.CurrentEnergy.Value++;

            StartCooldown(_cooldownDuration);
        }

        private void CheckSavedCooldown()
        {
            long currentServerTime = YG2.ServerTime() / 1000;
            long endTime = YG2.saves.RewardedAdvCooldownEndTime;

            if (endTime > currentServerTime)
                StartCooldown(endTime - currentServerTime);
            else
                EndCooldown();
        }

        private void StartCooldown(float duration)
        {
            _isCooldown = true;
            _currentCooldownTime = duration;
            _rewardedButton.interactable = false;
            _icon.gameObject.SetActive(false);
            _cooldownText.gameObject.SetActive(true);
        }

        private void EndCooldown()
        {
            _isCooldown = false;
            _rewardedButton.interactable = true;
            _icon.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(false);

            if (YG2.saves.RewardedAdvCooldownEndTime != 0)
            {
                YG2.saves.RewardedAdvCooldownEndTime = 0;
                YG2.SaveProgress();
            }
        }
    }
}