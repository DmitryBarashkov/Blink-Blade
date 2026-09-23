using System;
using System.Collections.Generic;
using BlinkBlade.Game;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class LanguagePanel : MonoBehaviour
    {
        private const float ExpandedWidth = 330f;
        private const float CollapsedWidth = 0;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _animationDuration = 0.3f;

        [Inject] private IAudioService _audioService;

        private RectTransform _rectTransform;

        private bool _isOpen = false;

        private Ease _easeType = Ease.InOutQuad;

        public event Action LanguageChanged;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(CollapsedWidth, _rectTransform.sizeDelta.y);

            _canvasGroup.alpha = 0f;
            SetInteraction(false);
        }

        public void ToggleMenu()
        {
            _audioService.PlaySound(SoundType.ExpandPanel);
            _rectTransform.DOKill();
            _canvasGroup.DOKill();

            if (!_isOpen)
            {
                _rectTransform.DOSizeDelta(new Vector2(ExpandedWidth, _rectTransform.sizeDelta.y), _animationDuration).SetEase(_easeType);
                _canvasGroup.DOFade(1f, _animationDuration);

                SetInteraction(true);
                _isOpen = true;
            }
            else
            {
                _rectTransform.DOSizeDelta(new Vector2(CollapsedWidth, _rectTransform.sizeDelta.y), _animationDuration).SetEase(_easeType);
                _canvasGroup.DOFade(0f, _animationDuration);

                SetInteraction(false);
                _isOpen = false;
            }
        }

        public void CreateSetLanguageButtons(SetLanguageButton prefab, string currentLanguage, Dictionary<string, Sprite> languages)
        {
            SetLanguageButton firstButton = Instantiate(prefab, this.transform);

            firstButton.Initialize(currentLanguage, languages[currentLanguage], this, _audioService);

            foreach (var language in languages)
            {
                if (language.Key == currentLanguage)
                    continue;

                SetLanguageButton setLanguageButton = Instantiate(prefab, this.transform);

                setLanguageButton.Initialize(language.Key, language.Value, this, _audioService);
            }
        }

        public void ChangeLanguage(string language)
        {
            LanguageChanged?.Invoke();

            SortSetLanguageButtons(language);
        }

        private void SortSetLanguageButtons(string language)
        {
            SetLanguageButton[] buttons = GetComponentsInChildren<SetLanguageButton>();

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetLanguageKey() == language)
                {
                    buttons[i].transform.SetAsFirstSibling();
                    break;
                }
            }
        }

        private void SetInteraction(bool state)
        {
            _canvasGroup.interactable = state;
            _canvasGroup.blocksRaycasts = state;
        }
    }
}