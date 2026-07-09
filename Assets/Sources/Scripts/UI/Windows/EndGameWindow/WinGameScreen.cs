using DG.Tweening;
using TMPro;
using UnityEngine;
using YG;
using Zenject;

public class WinGameScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemiesCountText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [SerializeField] private TextMeshProUGUI _levelNumberText;

    [SerializeField] private AddCoinsButton _addCoinsButton;
    [SerializeField] private AddEnergyButton _addEnergyButton;

    [Inject] private Level _level;

    private int _coinsFactor = 5;
    private float _effectDuration = 1f;
    private int _earnedCoins;

    private void OnEnable()
    {
        int enemiesCount = _level.EnemiesCount;
        int levelNumber = _level.LevelNumber;
        
        _earnedCoins = enemiesCount * _coinsFactor;

        _enemiesCountText.text = $"x{enemiesCount}";
        _coinsCountText.text = $"+{_earnedCoins}";
        _levelNumberText.text = levelNumber.ToString();

        _addCoinsButton.SetEnabled(true);
        _addEnergyButton.SetEnabled(true);

        YG2.saves.coins += _earnedCoins;
        YG2.saves.rating += _earnedCoins;
        YG2.saves.level++;
        YG2.SaveProgress();
        YG2.SetLeaderboard("Score", YG2.saves.rating);
    }

    public void AddCoins(int coinsMultiplier)
    {
        int currentCoins = _earnedCoins;
        
        _earnedCoins *= coinsMultiplier;

        DOTween.To(() => currentCoins, x => currentCoins = x, _earnedCoins, _effectDuration)
            .OnUpdate(() =>
            {
                _coinsCountText.text = $"+{currentCoins}";
            })
            .SetEase(Ease.OutQuad);

        YG2.saves.coins += _earnedCoins - currentCoins;
        YG2.SaveProgress();        
    }
}
