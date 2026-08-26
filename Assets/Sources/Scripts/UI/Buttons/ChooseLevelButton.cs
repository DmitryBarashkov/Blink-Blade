using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using YG;
using Zenject;

public class ChooseLevelButton : UIButton
{
    [SerializeField] private TextMeshProUGUI _input;
    [SerializeField] private RectTransform _screen;

    [Inject] private LevelLoadService _service;

    public override void HandleClick()
    {
        _screen.gameObject.SetActive(false);

        string cleanText = Regex.Replace(_input.text.Trim(), @"[^\d]", string.Empty);

        if (int.TryParse(cleanText, out int levelNumber))
        {
            YG2.saves.Level = levelNumber;
            _service.LoadLevel(levelNumber).Forget();
        }
    }
}
