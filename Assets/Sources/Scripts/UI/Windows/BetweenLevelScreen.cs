using TMPro;
using UniRx;
using UnityEngine;
using YG;
using Zenject;

public class BetweenLevelScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private RectTransform _chooseLevelScreen;

    [Inject] private InputService _input;
    [Inject] private PlayerStats _playerStats;

    private GameObject _gameObject;

    private void Awake()
    {
        _gameObject = gameObject;

        _playerStats.CurrentCoins.Subscribe((newCoins) =>
        {
            _coinsText.text = GetCoinText(newCoins);
        })
        .AddTo(this);
    }

    private void OnEnable()
    {
        _levelNumber.text = YG2.saves.Level.ToString();
        _coinsText.text = GetCoinText(_playerStats.CurrentCoins.Value);
        _input.ChooseLevelBtnPressed += ChooseLevel;
    }

    private void OnDisable()
    {
        _input.ChooseLevelBtnPressed -= ChooseLevel;
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

    private string GetCoinText(int coins)
    {
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

    private void ChooseLevel()
    {
        _chooseLevelScreen.gameObject.SetActive(true);
    }
}
