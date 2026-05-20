using TMPro;
using UnityEngine;
using YG;

public class BetweenLevelScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private TextMeshProUGUI _coinsText;
    
    private void OnEnable()
    {
        _levelNumber.text = YG2.saves.level.ToString();
        _coinsText.text = YG2.saves.coins.ToString();
    }
}
