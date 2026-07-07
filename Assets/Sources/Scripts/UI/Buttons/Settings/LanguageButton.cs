using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class LanguageButton : UIButton
{
    [SerializeField] private Image _image;
    [SerializeField] private LanguagePanel _panel;
    [SerializeField] private SetLanguageButton _setLanguageButton;
    
    private Dictionary<string, Sprite> _languages;

    [Inject]
    private void Construct([Inject(Id = "Languages")] Dictionary<string, Sprite> languages)
    {
        _languages = languages;

        SetLanguage();
        InitializeLanguageButtons();
    }

    protected override void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
        _panel.LanguageChanged += SetLanguage;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
        _panel.LanguageChanged -= SetLanguage;
    }

    public override void HandleClick()
    {
        _panel.ToggleMenu();
    }

    private void SetLanguage()
    {
        _image.sprite = _languages[YG2.lang];
    }

    private void InitializeLanguageButtons()
    {
        _panel.CreateSetLanguageButtons(_setLanguageButton, YG2.lang, _languages);
    }
}
