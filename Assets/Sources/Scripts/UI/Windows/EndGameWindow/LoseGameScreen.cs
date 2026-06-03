using TMPro;
using UnityEngine;
using Zenject;

public class LoseGameScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumber;

    [Inject] private Level _level;

    private void OnEnable()
    {
        _levelNumber.text = _level.LevelNumber.ToString();
    }
}
