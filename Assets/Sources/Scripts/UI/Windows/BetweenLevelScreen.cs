using TMPro;
using UnityEngine;
using YG;

public class BetweenLevelScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private NoAdsButton _noAdsButton;

    private void OnEnable()
    {
        _levelText.text = $"Level {YG2.saves.level}";

        if (YG2.saves.IsAdsDisabled == false)
        {
            Instantiate(_noAdsButton, transform);
        }

        _coinsText.text = YG2.saves.coins.ToString();
    }
}
