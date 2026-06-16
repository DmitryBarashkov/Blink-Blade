using TMPro;
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
        _coinsText.text = YG2.saves.coins.ToString();
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
}
