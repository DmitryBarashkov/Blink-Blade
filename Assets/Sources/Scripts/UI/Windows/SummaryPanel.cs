using TMPro;
using UnityEngine;
using Zenject;

public class SummaryPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemiesCountText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Inject] private Level _level;

    private int _coinsFactor = 5;

    private void OnEnable()
    {
        int enemiesCount = _level.EnemiesCount;
        int levelNumber = _level.LevelNumber;
        int coinsEarned = enemiesCount * _coinsFactor;

        _enemiesCountText.text = $"x{enemiesCount}";
        _coinsCountText.text = $"+{coinsEarned}";
        _levelText.text = $"Level {levelNumber} completed";
    }
}
