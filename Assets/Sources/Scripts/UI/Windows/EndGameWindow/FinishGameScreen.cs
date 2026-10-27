using TMPro;
using UnityEngine;
using YG;

namespace BlinkBlade.UI
{
    public class FinishGameScreen : EndGameScreen
    {
        [SerializeField] private RectTransform _panelTransform;
        [SerializeField] private RateButton _button;

        [SerializeField] private TextMeshProUGUI _coinCountText;

        private int _endGameReward = 500;

        private float _heightWithRateButton = 1050f;
        private float _heightWithoutRateButton = 940f;

        public override void Setup()
        {
            if (YG2.reviewCanShow == true)
            {
                _panelTransform.sizeDelta = new Vector2(_panelTransform.sizeDelta.x, _heightWithRateButton);
                _button.gameObject.SetActive(true);
            }
            else
            {
                _panelTransform.sizeDelta = new Vector2(_panelTransform.sizeDelta.x, _heightWithoutRateButton);
                _button.gameObject.SetActive(false);
            }

            _coinCountText.text = $"x{_endGameReward}";

            base.Setup();
        }

        private void OnEnable()
        {
            YG2.saves.Coins += _endGameReward;
            YG2.saves.Level += 1;
            YG2.SaveProgress();
            YG2.SetLeaderboard("Score", YG2.saves.Coins);
        }
    }
}