using TMPro;
using UnityEngine;
using YG;

public class LevelScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _levelText;

    private void OnEnable()
    {
        _levelText.text = $"Level {YG2.saves.level}";
    }
}
