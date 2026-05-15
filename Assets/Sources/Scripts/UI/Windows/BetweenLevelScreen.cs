using TMPro;
using UnityEngine;
using YG;

public class BetweenLevelScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private NoAdsButton _noAdsButton;
    
    private void OnEnable()
    {
        _levelNumber.text = YG2.saves.level.ToString();
        
        if (YG2.saves.IsAdsDisabled == false)
        {
            Instantiate(_noAdsButton, transform);
        }

        _coinsText.text = YG2.saves.coins.ToString();
    }
}
