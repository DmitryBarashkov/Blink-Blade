using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class BetweenLevelScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private TextMeshProUGUI _coinsText;

    private GameObject _gameObject;

    private void Awake()
    {
        _gameObject = gameObject;
    }

    private void OnEnable()
    {
        _levelNumber.text = YG2.saves.level.ToString();
        _coinsText.text = GetCoinText();
    }

    public void Activate()
    {
        if (_gameObject != null)
            _gameObject.SetActive(true);
        else
            gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        if (_gameObject != null)
            _gameObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    private string GetCoinText()
    {
        int coins = YG2.saves.coins;

        if (coins < 1000)
            return coins.ToString();

        if (coins < 1000000)
        {
            float rounded = Mathf.Floor((coins / 1000f) * 10f) / 10f;
            
            return (coins / 1000f).ToString("F1") + "k";
        }
        else
        {
            float rounded = Mathf.Floor((coins / 100000f) * 10f) / 10f;
            
            return (coins / 1000000f).ToString("F1") + "m";
        }
    }
}
