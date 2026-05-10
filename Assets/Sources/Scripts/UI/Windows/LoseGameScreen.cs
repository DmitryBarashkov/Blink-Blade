using TMPro;
using UnityEngine;
using Zenject;

public class LoseGameScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [Inject] private Level _level;

    private void OnEnable()
    {
        int levelNumber = _level.LevelNumber;

        _levelText.text = $"Level {levelNumber} failed";
    }
}
